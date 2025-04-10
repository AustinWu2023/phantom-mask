using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using phantom_mask.Entities;

namespace phantom_mask.Features.Users.Purchase {
    internal sealed class Endpoint : Endpoint<Request, Response> {

        public required AppDbContext DbContext { get; set; }
        public override void Configure() {
            Post("/purchase");
            AllowAnonymous();
            Summary(s =>
            {
                s.Summary = "Process a user purchases a mask from a pharmacy, and handle all relevant data changes in an atomic transaction.";
                s.Description = "Process a user purchases a mask from a pharmacy, and handle all relevant data changes in an atomic transaction.";
                s.ExampleRequest = new Request {
                    UserName = "Ada Larson",
                    PharmacyName = "First Care Rx",
                    MasksList = ["Second Smile (blue) (10 per pack)"]
                };
            });            

        }

        /// <summary>
        ///  1.檢查有沒有該使用者，如果沒有該使用者，回傳沒有使用者
        ///  2.檢查有沒有藥局，如果沒有藥局，回傳沒有藥局
        ///  3.用使用者口罩清單，去檢查該藥局有沒有符合的商品，只要缺一樣就回傳該藥局缺少使用者要購買的口罩
        ///  4.從藥局符合的商品中，計算使用者這次購物的金額，是否足夠用CashBalance扣除，如果不夠回傳帳戶餘額不足
        ///  5.如果都足夠，更新user 的 CashBalance，並購物的金額加到藥局的 CashBalance
        ///  6.將使用者購物的紀錄更新到PurchaseHistory裡面
        /// </summary>        
        public override async Task HandleAsync(Request r, CancellationToken c) {

            var user = await DbContext.Users
                        .Include(u => u.PurchaseHistories)
                        .FirstOrDefaultAsync(u => u.Name == r.UserName, c);

            if (user is null) {                
                await SendAsync(new Response { Message = $"User '{r.UserName}' not found." }, StatusCodes.Status404NotFound);
                return;
            }

            var pharmacy = await DbContext.Pharmacies
                .Include(p => p.Masks)
                .FirstOrDefaultAsync(p => p.PharmacyName == r.PharmacyName, c);

            if (pharmacy is null) {
                await SendAsync(new Response { Message = $"Pharmacy '{r.PharmacyName}' not found." }, StatusCodes.Status404NotFound);
                return;
            }

            var matchedMasks = pharmacy.Masks
                .Where(m => r.MasksList.Contains(m.ProductName))
                .ToList();

            if (matchedMasks.Count != r.MasksList.Count) {
                await SendAsync(new Response { Message = "Some requested masks are not available in the pharmacy." }, StatusCodes.Status404NotFound);
                return;
            }

            var totalAmount = matchedMasks.Sum(m => m.Price);

            if (user.CashBalance < totalAmount) {
                await SendAsync(new Response { Message = "Insufficient user balance." });
                return;
            }

            // Update balances
            user.DeductCashBalance(totalAmount);
            pharmacy.AddCashBalance(totalAmount);

            // Create purchase histories
            var purchaseHistories = matchedMasks.Select(m =>
                PurchaseHistory.Create(user, pharmacy, m, m.Price, DateTime.UtcNow)).ToList();

            user.AddPurchaseHistory(purchaseHistories);

            // Save to DB
            await DbContext.SaveChangesAsync(c);

            await SendAsync(new Response() { Message = "Purchase completed successfully." } );
            
        }
    }
}
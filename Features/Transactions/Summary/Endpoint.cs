using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using phantom_mask.Entities;

namespace phantom_mask.Features.Transactions.Summary {
    internal sealed class Endpoint : Endpoint<Request, Response> {
        public required AppDbContext DbContext { get; set; }
        public override void Configure() {
            Get("/transactions/summary");
            AllowAnonymous();
            Summary(s => {
                s.Summary = "The total amount of masks and dollar value of transactions within a date range.";
                s.Description = "The total amount of masks and dollar value of transactions within a date range.";
                s.ExampleRequest = new Request() {                    
                    StartDate = new DateTime(2021, 01, 24),
                    EndDate = new DateTime(2021, 01, 25)
                };
            });
        }

        public override async Task HandleAsync(Request r, CancellationToken c) {

            var transactions = await DbContext.PurchaseHistories
                        .Where(w => w.TransactionDate >= r.StartDate 
                                    && w.TransactionDate <= r.EndDate
                        ).ToListAsync();

            var result = new TransactionInfo
            {
                TotalTransactions = transactions.Count,
                TotalTransactionAmount = transactions.Sum(t => t.TransactionAmount)
            };            

            await SendAsync(new Response() { TransactionInfo = result } );
        }
    }
}
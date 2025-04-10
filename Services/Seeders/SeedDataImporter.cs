using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using phantom_mask.Entities;
using phantom_mask.Share.JsonDto;
using phantom_mask.Share.Options;
using phantom_mask.Utilities;
using System.Text.Json;

namespace phantom_mask.Services.Seeders {
    public class SeedDataImporter {

        private readonly AppDbContext _dbContext;
        private readonly string _pharmacyPath;
        private readonly string _userPath;
        private readonly IOpeningHourParser _openingHourParser;

        public SeedDataImporter(AppDbContext dbContext, IOptions<SeedDataOptions> options, IWebHostEnvironment env, IOpeningHourParser openingHourParser) {
            _dbContext = dbContext;
            _pharmacyPath = Path.Combine(env.ContentRootPath, options.Value.PharmacyJsonPath);
            _userPath = Path.Combine(env.ContentRootPath, options.Value.UserJsonPath);
            _openingHourParser = openingHourParser;
        }


        public async Task ImportIfEmptyAsync() {
            var hasPharmacy = await _dbContext.Pharmacies.AnyAsync();
            if (!hasPharmacy) {
                await ImportPharmaciesAsync();
            }

            var hasUser = await _dbContext.Users.AnyAsync();
            if (!hasUser) {
                await ImportUsersAsync();
            }
        }

        private async Task ImportPharmaciesAsync() {
            var json = await File.ReadAllTextAsync(_pharmacyPath);
            var pharmacyJsonList = JsonSerializer.Deserialize<List<PharmacySeedDto>>(json);
            
            var pharmacies = new List<Pharmacy>();
            foreach (var dto in pharmacyJsonList) {
                
                var masks = dto.masks?.Select(m => Mask.Create(m.name, m.price)).ToList();
                
                var pharmacy = Pharmacy.Create(dto.name, dto.cashBalance);
                
                if (masks.Any()) {
                    pharmacy.AddMaks(masks);
                }
                
                if (!string.IsNullOrWhiteSpace(dto.openingHours)) {
                    var openingHours = _openingHourParser.Parse(dto.openingHours, pharmacy);
                    pharmacy.AddOpeningHours(openingHours);
                }
                
                pharmacies.Add(pharmacy);
            }

            await _dbContext.Pharmacies.AddRangeAsync(pharmacies);
            await _dbContext.SaveChangesAsync();

        }

        private async Task ImportUsersAsync() {
            var json = await File.ReadAllTextAsync(_userPath);
            var usersJsonList = JsonSerializer.Deserialize<List<UserSeedDto>>(json);
            var marks = await _dbContext.Masks.ToListAsync();
            var pharmacies = await _dbContext.Pharmacies.ToListAsync();

            var users = new List<User>();
            foreach (var dto in usersJsonList) {

                var user = User.Create(dto.name, dto.cashBalance);

                if (dto.purchaseHistories?.Any() == true) {

                    var PurchaseHistories = new List<PurchaseHistory>();

                    foreach (var history in dto.purchaseHistories) {
                        var mark = marks.Where(w => w.ProductName == history.maskName).FirstOrDefault();
                        var pharmacy = pharmacies.Where(w => w.PharmacyName == history.pharmacyName).FirstOrDefault();
                        DateTime parsedDate = DateTime.Parse(history.transactionDate);

                        PurchaseHistories.Add(PurchaseHistory.Create(user, pharmacy, mark, history.transactionAmount, parsedDate));
                    }                   

                    user.AddPurchaseHistory(PurchaseHistories);
                }

                users.Add(user);
            }

            await _dbContext.Users.AddRangeAsync(users);
            await _dbContext.SaveChangesAsync();

        }
    }
}

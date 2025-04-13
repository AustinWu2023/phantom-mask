using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using phantom_mask.Entities;

namespace phantom_mask.Features.Pharmacies.Filter {
    internal sealed class Endpoint : Endpoint<Request, Response> {
        public required AppDbContext DbContext { get; set; }
        public override void Configure() {
            Get("/pharmacies/filter");
            AllowAnonymous();
            Summary(s => {
                s.Summary = "List all pharmacies with more or less than x mask products within a price range.";
                s.Description = "List all pharmacies with more or less than x mask products within a price range.";
                s.ExampleRequest = new Request {
                    ProductCount = 3,
                    MoreThan = true,     // true => 口罩數量超過
                    MinPrice = 20m,     
                    MaxPrice = 100m
                };
            });
        }

        public override async Task HandleAsync(Request r, CancellationToken c) {

            var pharmacies = await DbContext.Pharmacies
                             .Include(p => p.Masks)
                             .ToListAsync(c);

            var filteredPharmacies = pharmacies
                .Select(p => new {
                    Pharmacy = p,
                    FilteredMasks = p.Masks
                        .Where(m => m.Price >= r.MinPrice && m.Price <= r.MaxPrice)
                        .ToList()
                })
                .Where(x =>
                    r.MoreThan
                        ? x.FilteredMasks.Count > r.ProductCount
                        : x.FilteredMasks.Count < r.ProductCount)
                .ToList();

            var response = new Response {
                Pharmacies = filteredPharmacies
                    .Where(x => x.FilteredMasks.Any())
                    .Select(x => new PharmacyInfo {
                        PharmacyName = x.Pharmacy.PharmacyName,
                        Masks = x.FilteredMasks.Select(m => new MaskInfo {
                            ProductName = m.ProductName,
                            Price = m.Price
                        }).ToList()
                    }).ToList()
            };


            await SendAsync(response);
        }
    }
}
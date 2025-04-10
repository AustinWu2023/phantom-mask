using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using phantom_mask.Entities;

namespace phantom_mask.Features.Pharmacies.Masks {
    internal sealed class Endpoint : Endpoint<Request, Response> {
        public required AppDbContext DbContext { get; set; }
        public override void Configure() {
            Get("/pharmacies/masks");
            AllowAnonymous();
            Summary(s => {
                s.Summary = "List all masks sold by a given pharmacy, sorted by mask name or price";
                s.Description = "List all masks sold by a given pharmacy, sorted by mask name or price";
                s.ExampleRequest = new Request {
                    PharmacyName = "HealthMart",
                    SortBy = "price"
                };
            });
        }

        public override async Task HandleAsync(Request r, CancellationToken c) {

            var result = await DbContext.Pharmacies
                        .Include(p => p.Masks)
                        .FirstOrDefaultAsync(p => p.PharmacyName == r.PharmacyName, c);

            if (result == null) {
                await SendNotFoundAsync();
                return;
            }

            var sortBy = r.SortBy?.ToLower() ?? "name"; // 預設為name

            var sortedMasks = sortBy switch {
                "price" => result.Masks.OrderBy(m => m.Price),
                _ => result.Masks.OrderBy(m => m.ProductName)
            };

            var response = new Response {
                pharmacyInfo = new PharmacyInfo {
                    PharmacyName = result.PharmacyName,
                    Masks = sortedMasks.Select(m => new MaskInfo {
                        ProductName = m.ProductName,
                        Price = m.Price
                    }).ToList()
                }
            };
            await SendAsync(response);
        }
    }
}
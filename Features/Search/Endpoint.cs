using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using phantom_mask.Entities;

namespace phantom_mask.Features.Search {
    internal sealed class Endpoint : Endpoint<Request, Response> {
        public required AppDbContext DbContext { get; set; }

        public override void Configure() {
            Post("/search");
            AllowAnonymous();
            Summary(s => {
                s.Summary = "Search for pharmacies or masks by name, ranked by relevance to the search term.";
                s.Description = "Search for pharmacies or masks by name, ranked by relevance to the search term.";
                s.ExampleRequest = new Request {
                    Keyword = "Care"
                };
            });
        }

        public override async Task HandleAsync(Request r, CancellationToken c) {

            var keyword = r.Keyword.Trim().ToLower();

            var pharmacies = await DbContext.Pharmacies
                .Include(p => p.Masks)
                .ToListAsync(c);

            var matched = pharmacies
                .Select(p => new {
                    Pharmacy = p,
                    MatchedMasks = p.Masks
                        .Where(m => m.ProductName.ToLower().Contains(keyword)).ToList(),
                    PharmacyNameMatch = p.PharmacyName.ToLower().Contains(keyword)
                })
                .Where(x => x.PharmacyNameMatch || x.MatchedMasks.Any())
                .ToList();

            var response = new Response {
                Pharmacies = matched.Select(x => new PharmacyInfo {
                    PharmacyName = x.Pharmacy.PharmacyName,
                    Masks = x.MatchedMasks.Select(m => new MaskInfo {
                        ProductName = m.ProductName,
                        Price = m.Price
                    }).ToList()
                }).ToList()
            };

            await SendAsync(response);
        }
    }
}
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using phantom_mask.Entities;

namespace phantom_mask.Features.Pharmacies.Open {
    internal sealed class Endpoint : Endpoint<Request, Response> {

        public required AppDbContext DbContext { get; set; }

        public override void Configure() {
            Get("/pharmacies/open/");
            AllowAnonymous();
            Summary(s => {
                s.Summary = "List pharmacies open at a specific time and weekday";
                s.Description = "List pharmacies open at a specific time and weekday";
                s.ExampleRequest = new Request {
                    DayOfWeek = "Mon",
                    Time = "21:30",
                    IsOvernight = false
                };
            });
        }

        public override async Task HandleAsync(Request r, CancellationToken c) {

            var result = await DbContext.OpeningHours.Include(i => i.Pharmacy)
                         .Where(w => w.IsOvernight == r.IsOvernight 
                                        && w.StartDayOfWeek == r.DayOfWeek 
                                        && w.OpenTime == r.Time)
                         .Select(s => new PharmacInfo {
                             PharmacyName = s.Pharmacy.PharmacyName
                         })
                         .ToListAsync();

            await SendAsync(new Response() { PharmacInfos = result });
        }
    }
}
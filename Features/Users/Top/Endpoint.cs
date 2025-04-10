using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using phantom_mask.Entities;

namespace phantom_mask.Features.Users.Top {
    internal sealed class Endpoint : Endpoint<Request, Response> {

        public required AppDbContext DbContext { get; set; }

        public override void Configure() {
            Post("/users/top");
            AllowAnonymous();
            Summary(s => {
                s.Summary = "The top x users by total transaction amount of masks within a date range.";
                s.Description = "The top x users by total transaction amount of masks within a date range.";
                s.ExampleRequest = new Request() {
                    TopX = 1,
                    StartDate = new DateTime(2021, 01, 24),
                    EndDate = new DateTime(2021, 01, 25)
                };
            });
        }

        public override async Task HandleAsync(Request r, CancellationToken c) {

            var result = await DbContext.Users
                .Select(user => new {
                    user.Name,
                    TotalSpent = user.PurchaseHistories
                        .Where(h => h.TransactionDate >= r.StartDate && h.TransactionDate <= r.EndDate)
                        .Sum(h => h.TransactionAmount)
                })
                .OrderByDescending(x => x.TotalSpent)
                .Take(r.TopX)
                .ToListAsync(c);

            var response = new Response {
                TopUserInfos = result.Select(x => new TopUserInfo {
                    UserName = x.Name,
                    TotalSpent = x.TotalSpent
                }).ToList()
            };


            await SendAsync(response);
        }
    }
}
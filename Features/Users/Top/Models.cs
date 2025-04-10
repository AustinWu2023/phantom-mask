using FastEndpoints;
using FluentValidation;

namespace phantom_mask.Features.Users.Top {
    internal sealed class Request {
        public int TopX { get; set; }        
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }        
    }

    internal sealed class Validator : Validator<Request> {
        public Validator() {
            RuleFor(x => x.TopX)
           .GreaterThan(0)
           .WithMessage("TopX must be a positive integer.");

            RuleFor(x => x.StartDate)
                .Must(BeAValidDate)
                .WithMessage("StartDate must be a valid date.");

            RuleFor(x => x.EndDate)
                .Must(BeAValidDate)
                .WithMessage("EndDate must be a valid date.");

            RuleFor(x => x)
                .Must(x => x.EndDate > x.StartDate)
                .WithMessage("EndDate must be later than StartDate.");
        }

        private bool BeAValidDate(DateTime date) {
            return date != default;
        }
    }

    internal sealed class Response {
        public List<TopUserInfo> TopUserInfos { get; set; } = [];
    }

    public class TopUserInfo {
        public string UserName { get; set; } = string.Empty;
        public decimal TotalSpent { get; set; }
    }
}

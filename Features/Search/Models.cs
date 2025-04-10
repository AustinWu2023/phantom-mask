using FastEndpoints;
using FluentValidation;

namespace phantom_mask.Features.Search {
    internal sealed class Request {
        public string Keyword { get; set; } = string.Empty;
    }

    internal sealed class Validator : Validator<Request> {
        public Validator() {
            RuleFor(x => x.Keyword)
                .NotEmpty()
                .WithMessage("Search keyword is required.")
                .MinimumLength(2)
                .WithMessage("Search keyword must be at least 2 characters long.");
        }
    }

    internal sealed class Response {
        public List<PharmacyInfo> Pharmacies { get; set; } = [];
    }

    public class PharmacyInfo {
        public string PharmacyName { get; set; } = string.Empty;
        public List<MaskInfo> Masks { get; set; } = [];
    }

    public class MaskInfo {
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}

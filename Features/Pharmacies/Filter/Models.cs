using FastEndpoints;
using FluentValidation;

namespace phantom_mask.Features.Pharmacies.Filter {
    internal sealed class Request {
        public int ProductCount { get; set; }
        public bool MoreThan { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }       
    }

    internal sealed class Validator : Validator<Request> {
        public Validator() {
            RuleFor(x => x.ProductCount)
               .GreaterThanOrEqualTo(0)
               .WithMessage("ProductCount must be a non-negative number.");

            RuleFor(x => x.MinPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("MinPrice must be greater than or equal to 0.");

            RuleFor(x => x.MaxPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("MaxPrice must be greater than or equal to 0.");

            RuleFor(x => x)
                .Must(x => x.MaxPrice >= x.MinPrice)
                .WithMessage("MaxPrice must be greater than or equal to MinPrice.");

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

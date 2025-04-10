using FastEndpoints;
using FluentValidation;

namespace phantom_mask.Features.Pharmacies.Masks {
    internal sealed class Request {              
        public string PharmacyName { get; set; } = string.Empty;        
        public string SortBy { get; set; } = "name"; // or "price"
    }

    internal sealed class Validator : Validator<Request> {
        public Validator() {
            RuleFor(x => x.PharmacyName)
            .NotEmpty()
            .WithMessage("PharmacyName is required.");

            RuleFor(x => x.SortBy.ToLower())
                .Must(s => s == "name" || s == "price")
                .WithMessage("SortBy must be either 'name' or 'price'.");
        }
    }

    internal sealed class Response {
        public PharmacyInfo pharmacyInfo { set; get; } = new PharmacyInfo();
    }

    public class PharmacyInfo() {
        public string PharmacyName { get; set; } = string.Empty;
        public List<MaskInfo> Masks { get; set; } = new List<MaskInfo>();
    }

    public class MaskInfo() {
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}

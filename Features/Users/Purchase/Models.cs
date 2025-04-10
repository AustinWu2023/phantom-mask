using FastEndpoints;
using FluentValidation;

namespace phantom_mask.Features.Users.Purchase {
    internal sealed class Request {        

        public string UserName { get; set; } = string.Empty;
        public string PharmacyName { get; set; } = string.Empty;
        public List<string> MasksList { get; set; } = [];
        
    }

    internal sealed class Validator : Validator<Request> {
        public Validator() {
            RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("UserName is required.");

            RuleFor(x => x.PharmacyName)
                .NotEmpty()
                .WithMessage("PharmacyName is required.");

            RuleFor(x => x.MasksList)
                .NotNull()
                .WithMessage("MasksList must be provided.")
                .Must(m => m.Count > 0)
                .WithMessage("MasksList cannot be empty.")
                .Must(m => m.All(name => !string.IsNullOrWhiteSpace(name)))
                .WithMessage("Mask names in the list cannot be empty.");
        }
    }

    internal sealed class Response {
        public string Message { get; set; } = string.Empty;
    }

    

}

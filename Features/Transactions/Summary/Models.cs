using FastEndpoints;
using FluentValidation;

namespace phantom_mask.Features.Transactions.Summary {
    internal sealed class Request {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }       
    }

    internal sealed class Validator : Validator<Request> {
        public Validator() {
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
        public TransactionInfo TransactionInfo { get; set; } = new TransactionInfo();
    }

    public class TransactionInfo() {
        public int TotalTransactions { get; set; } = 0;
        public decimal TotalTransactionAmount { get; set; }
    }

}

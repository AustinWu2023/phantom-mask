using FastEndpoints;
using FluentValidation;
using System.Globalization;

namespace phantom_mask.Features.Pharmacies.Open {
    internal sealed class Request {
        /// <example>Mon</example>
        public string DayOfWeek { get; set; } = string.Empty;

        /// <example>14:30</example>
        public string Time { get; set; } = string.Empty;

        /// <example>是否跨夜</example>
        public bool IsOvernight { get; set; } = false;
    }

    internal sealed class Validator : Validator<Request> {
        public Validator() {
            RuleFor(x => x.DayOfWeek)
                .NotEmpty().WithMessage("Day of week is required.")
                .Must(BeAValidDay).WithMessage("Day of week must be a valid abbreviation: Mon, Tue, Wed, Thu, Fri, Sat, or Sun.");

            RuleFor(x => x.Time)
                .NotEmpty().WithMessage("Time is required.")
                .Must(BeValidTime).WithMessage("Time must be in HH:mm format, e.g., 14:30.");
        }

        private bool BeAValidDay(string day) {
            var validDays = new[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            return validDays.Contains(day?.Trim(), StringComparer.OrdinalIgnoreCase);
        }

        private bool BeValidTime(string time) {
            return TimeSpan.TryParseExact(time, "hh\\:mm", CultureInfo.InvariantCulture, out _)
                || TimeSpan.TryParseExact(time, "h\\:mm", CultureInfo.InvariantCulture, out _)
                || TimeSpan.TryParseExact(time, "HH\\:mm", CultureInfo.InvariantCulture, out _); // 更容錯
        }
    }

    internal sealed class Response {
        public List<PharmacInfo> PharmacInfos { get; set; } = [];
    }

    public class PharmacInfo() {
        public string PharmacyName { get; set; } = string.Empty;
    }
}

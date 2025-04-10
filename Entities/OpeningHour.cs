using System.ComponentModel.DataAnnotations;

namespace phantom_mask.Entities {

    public class OpeningHour : BaseEntity {
        public Pharmacy Pharmacy { get; private set; }

        public string StartDayOfWeek { get; private set; } = string.Empty;
        public string EndDayOfWeek { get; private set; } = string.Empty;

        public string OpenTime { get; private set; } = string.Empty;
        public string CloseTime { get; private set; } = string.Empty;

        public bool IsOvernight { get; private set; }

        private OpeningHour() { }

        private OpeningHour(Pharmacy pharmacy, string startDay, string endDay, string openTime, string closeTime) {
            Pharmacy = pharmacy;
            StartDayOfWeek = startDay;
            EndDayOfWeek = endDay;
            OpenTime = openTime;
            CloseTime = closeTime;
            IsOvernight = !StartDayOfWeek.Equals(EndDayOfWeek, StringComparison.OrdinalIgnoreCase);
        }

        public static OpeningHour Create(Pharmacy pharmacy, string startDay, string endDay, string openTime, string closeTime)
            => new OpeningHour(pharmacy, startDay, endDay, openTime, closeTime);
    }
}

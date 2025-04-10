using phantom_mask.Entities;

namespace phantom_mask.Utilities {
    public interface IOpeningHourParser {
        List<OpeningHour> Parse(string openingHourText, Pharmacy pharmacy);
    }
}

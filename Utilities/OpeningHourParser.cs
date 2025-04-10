using phantom_mask.Entities;
using System.Text.RegularExpressions;

namespace phantom_mask.Utilities {
    public class OpeningHourParser : IOpeningHourParser {
        public List<OpeningHour> Parse(string openingHourText, Pharmacy pharmacy) {
            var results = new List<OpeningHour>();
            if (string.IsNullOrWhiteSpace(openingHourText))
                return results;

            var segments = openingHourText.Split("/", StringSplitOptions.TrimEntries);

            foreach (var segment in segments) {
                var trimmed = segment.Trim().Replace("–", "-");

                // 抓出最後一組時間
                var match = Regex.Match(trimmed, @"(?<dayPart>.+?)\s+(?<open>\d{2}:\d{2})\s*-\s*(?<close>\d{2}:\d{2})");
                if (!match.Success) continue;

                var dayPart = match.Groups["dayPart"].Value.Trim();
                var openTime = match.Groups["open"].Value;
                var closeTime = match.Groups["close"].Value;

                var dayTokens = dayPart.Split(",", StringSplitOptions.TrimEntries);

                foreach (var token in dayTokens) {
                    var dayToken = token.Trim().Replace("–", "-");

                    if (dayToken.Contains("-")) {
                        var range = dayToken.Split("-", StringSplitOptions.TrimEntries);
                        if (range.Length == 2) {
                            var expanded = ExpandDayRange(range[0], range[1]);
                            foreach (var day in expanded) {
                                results.Add(OpeningHour.Create(pharmacy, day, day, openTime, closeTime));
                            }
                        }
                    } else {
                        // CloseTime < OpenTime 代表有跨夜
                        var normalizedDay = NormalizeDay(token);
                        var closeDay = IsOvernight(openTime, closeTime)
                            ? GetNextDay(normalizedDay)
                            : normalizedDay;

                        results.Add(OpeningHour.Create(pharmacy, normalizedDay, closeDay, openTime, closeTime));
                    }
                }
            }

            return results;
        }

        private static bool IsOvernight(string openTime, string closeTime) {
            return TimeSpan.Parse(closeTime) < TimeSpan.Parse(openTime);
        }

        private static string GetNextDay(string day) {
            var days = new List<string> { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            var index = days.IndexOf(NormalizeDay(day));
            if (index == -1) return day;
            return days[(index + 1) % 7];
        }

        private static string NormalizeDay(string day) {
            return day.Trim() switch {
                "Thur" => "Thu",
                "Thurs" => "Thu",
                "Tues" => "Tue",
                "Weds" => "Wed",
                _ => day.Trim()
            };
        }

        private static List<string> ExpandDayRange(string start, string end) {
            var days = new List<string> { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            var startIndex = days.IndexOf(NormalizeDay(start));
            var endIndex = days.IndexOf(NormalizeDay(end));

            if (startIndex < 0 || endIndex < 0 || startIndex > endIndex)
                return new List<string>();

            return days.GetRange(startIndex, endIndex - startIndex + 1);
        }
    }
}

using System.Text.RegularExpressions;

namespace TourPlanner.Services
{
    public partial class TimeFormatService
    {
        [GeneratedRegex(@"PT(?:(\d+)H)?(?:(\d+)M)?(?:(\d+)S)?")]
        private static partial Regex MyRegex();

        public static (int hours, int minutes) ParseIso8601DurationToTuple(string duration)
        {
            var result = GetHoursAndMinutes(duration);
            return result ?? (0, 0);
        }

        public static string FormatIso8601Duration(int hours, int minutes)
        {
            return $"PT{hours}H{minutes}M00S";
        }

        public static string ParseIso8601DurationToString(string duration)
        {
            var (hours, minutes) = GetHoursAndMinutes(duration) ?? (0, 0);
            return $"{hours} hours, {minutes} minutes";
        }

        private static (int hours, int minutes)? GetHoursAndMinutes(string duration)
        {
            var match = MyRegex().Match(duration);
            if (!match.Success) return null;
            
            var hours = match.Groups[1].Success ? int.Parse(match.Groups[1].Value) : 0;
            var minutes = match.Groups[2].Success ? int.Parse(match.Groups[2].Value) : 0;
            return (hours, minutes);
        }
    }
}
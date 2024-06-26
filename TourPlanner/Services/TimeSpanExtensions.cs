namespace TourPlanner.Services
{
    public static class TimeSpanExtensions
    {
        public static string Format(this TimeSpan timeSpan)
        {
            var parts = new List<string>();

            if (timeSpan.Days > 0)
                parts.Add($"{timeSpan.Days} day(s)");

            if (timeSpan.Hours > 0)
                parts.Add($"{timeSpan.Hours} hour(s)");

            if (timeSpan.Minutes > 0)
                parts.Add($"{timeSpan.Minutes} minute(s)");

            if (timeSpan.Seconds > 0)
                parts.Add($"{timeSpan.Seconds} second(s)");

            return string.Join(" ", parts);
        }
    }
}
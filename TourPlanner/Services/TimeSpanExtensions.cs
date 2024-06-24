namespace TourPlanner.Services;

public static class TimeSpanExtensions
{
    public static string Format(this TimeSpan timeSpan)
    {
        return $"{timeSpan.Days} day(s) {timeSpan.Hours} hour(s) {timeSpan.Minutes} minute(s) {timeSpan.Seconds} second(s)";
    }
}

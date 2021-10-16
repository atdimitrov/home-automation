using System;

namespace HomeAutomation.Server
{
    public static class DateTimeExtensions
    {
        public static bool IsTimeAfter(this DateTime a, DateTime b) =>
            ConvertToTimeComparable(a) > ConvertToTimeComparable(b);

        public static bool IsTimeBefore(this DateTime a, DateTime b) =>
            ConvertToTimeComparable(a) < ConvertToTimeComparable(b);

        private static DateTime ConvertToTimeComparable(DateTime dateTime) =>
            new DateTime(1970, 1, 1, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, DateTimeKind.Utc);
    }
}

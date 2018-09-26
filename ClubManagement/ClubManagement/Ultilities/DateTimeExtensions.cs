using System;
using DateTime = System.DateTime;

namespace ClubManagement.Ultilities
{
    public static class DateTimeExtensions
    {
        private static DateTime NextWeekDay(DateTime start, DayOfWeek day)
        {
            var daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            if (day == DayOfWeek.Sunday || start.DayOfWeek <= day) daysToAdd += 7;
            return start.AddDays(daysToAdd);
        }

        public static string ToDateString(this DateTime time) => time.ToString("MMM dd, yyyy");

        public static string ToTimeString(this DateTime time) => time.ToString("h:mmtt");

        public static string ToDateTimeString(this DateTime startTime, DateTime endTime)
        {
            return startTime.Day == endTime.Day
                ? startTime.ToString($"MMM dd \u00B7 h{startTime.ToMinuteString()} tt") +
                    " - " +
                    endTime.ToString($"h{endTime.ToMinuteString()} tt")
                : startTime.ToString("MMM dd") + " - " + endTime.ToString("MMM dd");
        }

        private static string ToMinuteString(this DateTime time)
        {
            return time.Minute == 0
                ? ""
                : time.ToString(":mm");
        }

        public static bool IsInThisWeek(this DateTime dateTime)
        {
            var today = DateTime.Today;
            var thisSunday = NextWeekDay(today, DayOfWeek.Sunday).AddDays(-7);
            return dateTime.Date >= DateTime.Today && dateTime.Date <= thisSunday.Date;
        }

        public static bool IsInNextWeek(this DateTime dateTime)
        {
            var today = DateTime.Today;
            var nextWeekMonday = NextWeekDay(today, DayOfWeek.Monday);
            var nextWeekSunday = NextWeekDay(today, DayOfWeek.Sunday);
            return dateTime.Date >= nextWeekMonday.Date && dateTime.Date <= nextWeekSunday.Date;
        }

        public static bool IsTomorrow(this DateTime dateTime)
        {
            return dateTime.Date == DateTime.Today.AddDays(1).Date;
        }

        public static bool IsToday(this DateTime dateTime)
        {
            return dateTime.Date == DateTime.Today;
        }
    }
}
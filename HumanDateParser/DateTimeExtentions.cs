using System;

namespace HumanDateParser
{
    public static class DateTimeExtentions
    {
        public static DateTime SetTime(this DateTime time, int hour, int minute)
        {
            time = new DateTime(time.Year, time.Month, time.Day, hour, minute, 0);
            return time;
        }
    }
}
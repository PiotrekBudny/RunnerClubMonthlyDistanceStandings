using System;

namespace RunnersClubMonthlyDistanceStandings
{
    public static class TimeSpanExtensions
    {
        public static TimeSpan TrimMilisecondsAndSeconds(this TimeSpan timeSpanValue)
        {
            return new TimeSpan(timeSpanValue.Days, timeSpanValue.Hours, timeSpanValue.Minutes, timeSpanValue.Seconds);
        }
    }
}
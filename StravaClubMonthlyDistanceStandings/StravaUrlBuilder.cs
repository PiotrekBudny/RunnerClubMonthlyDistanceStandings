namespace StravaClubMonthlyDistanceStandings
{
    public static class StravaUrlBuilder
    {
        public static string ClubPage(string stravaEndpoint, string clubId) => $"{stravaEndpoint}/clubs/{clubId}";

        public static string MonthlyActivities(string profileUrl, string monthlyFilterValue) =>
            $"{profileUrl}#interval?interval={monthlyFilterValue}&interval_type=month&chart_type=miles&year_offset=0";
    }
}
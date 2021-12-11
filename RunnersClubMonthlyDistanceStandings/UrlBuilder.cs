namespace RunnersClubMonthlyDistanceStandings
{
    public static class UrlBuilder
    {
        public static string ClubPage(string clubEndpoint, string clubId) => $"{clubEndpoint}/clubs/{clubId}";

        public static string MonthlyActivities(string profileUrl, string monthlyFilterValue) =>
            $"{profileUrl}#interval?interval={monthlyFilterValue}&interval_type=month&chart_type=miles&year_offset=0";
    }
}
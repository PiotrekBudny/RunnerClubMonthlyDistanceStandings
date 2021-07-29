using System.Collections.Generic;

namespace StravaClubMonthlyDistanceStandings.Web.Models
{
    public class LatestMonthlyStandingsResponse
    {
        public string ReportDate { get; set; }
        public string ReportCode { get; set; }

        public string ReportWorkoutTypes { get; set; }

        public List<AthleteSummary> AthleteStandings { get; set; }
    }
}
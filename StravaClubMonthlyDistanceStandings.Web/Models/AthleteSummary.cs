namespace StravaClubMonthlyDistanceStandings.Web.Models
{
    public class AthleteSummary
    {
        public string CurrentPlace { get; set; }
        public string AthleteName { get; set; }
        public decimal DistanceSumInKilometers { get; set; }
        public decimal ElevationInMeters { get; set; }
        public string AvgPace { get; set; }
        public long TrainingCount { get; set; }
    }
}
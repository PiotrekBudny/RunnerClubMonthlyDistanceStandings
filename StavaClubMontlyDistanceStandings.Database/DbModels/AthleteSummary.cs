namespace StravaClubMonthlyDistanceStandings.Database.DbModels
{
    public class AthleteSummary
    {
        public long AthleteSummaryId { get; set; }
        public long MonthlySummaryId { get; set; }
        public string AthleteName { get; set; }
        public byte[] DistanceSumInKilometers { get; set; }
        public byte[] ElevationInMeters { get; set; }
        public string AvgPace { get; set; }
        public long TrainingCount { get; set; }

        public virtual MonthlySummary MonthlySummary { get; set; }
    }
}

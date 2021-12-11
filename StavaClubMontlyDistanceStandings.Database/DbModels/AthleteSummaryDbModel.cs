namespace StravaClubMonthlyDistanceStandings.Database.DbModels
{
    public class AthleteSummaryDbModel
    {
        public long AthleteSummaryId { get; set; }
        public long MonthlySummaryId { get; set; }
        public string AthleteName { get; set; }
        public decimal DistanceSumInKilometers { get; set; }
        public decimal ElevationInMeters { get; set; }
        public string AvgPace { get; set; }
        public long TrainingCount { get; set; }

        public virtual MonthlySummaryDbModel MonthlySummary { get; set; }
    }
}

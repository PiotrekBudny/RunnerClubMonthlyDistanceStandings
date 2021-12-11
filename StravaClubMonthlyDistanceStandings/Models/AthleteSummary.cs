using System;

namespace StravaClubMonthlyDistanceStandings.Models
{
    public class AthleteSummary
    {
        public string AthleteName { get; set; }
        public decimal Distance { get; set; }
        public string AveragePace { get; set; }
        public decimal ElevationSum { get; set; }
        public string TrainingType { get; set; }
        public int TrainingCount { get; set; }
        public bool IsEmployee { get; set; }
    }
}
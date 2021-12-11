using System;

namespace RunnersClubMonthlyDistanceStandings.Models
{
    public class ActivityDetails
    {
        public string AthleteName { get; set; }
        public decimal TrainingDistance { get; set; }
        public string TrainingType { get; set; }
        public decimal TrainingElevation { get; set; }
        public TimeSpan TrainingPace { get; set; }
    }
}
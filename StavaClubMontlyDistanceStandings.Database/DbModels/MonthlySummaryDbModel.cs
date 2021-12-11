using System.Collections.Generic;

namespace StravaClubMonthlyDistanceStandings.Database.DbModels
{
    public class MonthlySummaryDbModel
    {
        public MonthlySummaryDbModel()
        {
            AthleteSummaries = new HashSet<AthleteSummaryDbModel>();
        }
        public long MonthlySummaryId { get; set; }
        public string SummaryCode { get; set; }
        public string CreatedOn { get; set; }
        public string TrainingTypes { get; set; }
        public virtual ICollection<AthleteSummaryDbModel> AthleteSummaries { get; set; }
    }
}

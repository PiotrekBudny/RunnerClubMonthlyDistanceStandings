using System.Collections.Generic;

namespace StravaClubMonthlyDistanceStandings.Database.DbModels
{
    public class MonthlySummary
    {
        public MonthlySummary()
        {
            AthleteSummaries = new HashSet<AthleteSummary>();
        }

        public long MonthlySummaryId { get; set; }
        public string MonthlySummaryCode { get; set; }
        public string MontlhySummaryGenerationDate { get; set; }
        public string MonthlySummaryTrainingTypes { get; set; }

        public virtual ICollection<AthleteSummary> AthleteSummaries { get; set; }
    }
}

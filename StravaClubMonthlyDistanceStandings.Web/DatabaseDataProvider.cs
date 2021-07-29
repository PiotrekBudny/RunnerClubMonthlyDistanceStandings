using System.Collections.Generic;
using System.Linq;
using StravaClubMonthlyDistanceStandings.Database;
using StravaClubMonthlyDistanceStandings.Database.DbModels;
using StravaClubMonthlyDistanceStandings.Web.Models;

namespace StravaClubMonthlyDistanceStandings.Web
{
    public interface IDatabaseDataProvider
    {
        public LatestMonthlyStandingsResponse GetLatestMonthlyReport(string summaryCode);
    }
    
    public class DatabaseDataProvider : IDatabaseDataProvider
    {
        private SqLiteDbContext _dbContext;
        
        public DatabaseDataProvider(SqLiteDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        
        public LatestMonthlyStandingsResponse GetLatestMonthlyReport(string summaryCode)
        {
            using (_dbContext)
            {
                var dbHandler = new DbHandler(_dbContext);

                var latestSummary = dbHandler.GetLatestSummaryForSummaryCode(summaryCode);

                return new LatestMonthlyStandingsResponse()
                {
                    ReportCode = latestSummary.SummaryCode,
                    ReportDate = latestSummary.CreatedOn,
                    ReportWorkoutTypes = latestSummary.TrainingTypes,
                    AthleteStandings = AthleteSummariesMapper(latestSummary.AthleteSummaries)
                };
            }
        }


        private List<AthleteSummary> AthleteSummariesMapper(ICollection<AthleteSummaryDbModel> athleteSummaryDbModels)
        {
            var athleteSummaryList = new List<AthleteSummary>();
            var place = 1;
            foreach (var model in athleteSummaryDbModels)
            {
                athleteSummaryList.Add(new AthleteSummary()
                {
                    CurrentPlace = place.ToString(),
                    AthleteName = model.AthleteName,
                    AvgPace = model.AvgPace,
                    DistanceSumInKilometers = model.DistanceSumInKilometers,
                    ElevationInMeters = model.ElevationInMeters,
                    TrainingCount = model.TrainingCount
                });

                place++;
            }

            return athleteSummaryList;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StravaClubMonthlyDistanceStandings.Database.DbModels;

namespace StravaClubMonthlyDistanceStandings.Database
{
    public class DbHandler
    {
        private readonly SqLiteDbContext _dbContext;
        
        public DbHandler(SqLiteDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public void InsertMonthlySummaryToDatabase(MonthlySummaryDbModel monthlySummaryModel, List<AthleteSummaryDbModel> athleteSummariesModels)
        {
            try
            {
                 monthlySummaryModel.AthleteSummaries = athleteSummariesModels;

                _dbContext.Add(monthlySummaryModel);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to insert data to SqLiteDatabase = {0}, {1}", e.Message, e.InnerException?.Message);
            }
        }

        public MonthlySummaryDbModel GetLatestSummaryForSummaryCode(string summaryCode)
        {
            MonthlySummaryDbModel response;
            
            try
            {
                response = _dbContext.MonthlySummaries.Where(x => x.SummaryCode == summaryCode)
                    .OrderByDescending(x => x.CreatedOn).ToList().FirstOrDefault();

                if (response != null)
                    response.AthleteSummaries =
                        _dbContext.AthleteSummaries.Where(x => x.MonthlySummaryId == response.MonthlySummaryId).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
            
            return response;
        }
    }
}

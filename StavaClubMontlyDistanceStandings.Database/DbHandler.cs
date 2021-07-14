using System;
using System.Collections.Generic;
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
    }
}

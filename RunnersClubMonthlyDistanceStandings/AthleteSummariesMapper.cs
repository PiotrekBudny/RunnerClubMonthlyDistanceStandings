using System;
using System.Collections.Generic;
using System.Linq;
using RunnersClubMonthlyDistanceStandings.Database.DbModels;
using RunnersClubMonthlyDistanceStandings.Models;

namespace RunnersClubMonthlyDistanceStandings
{
    public class AthleteSummariesMapper
    {
        public List<AthleteSummaryDbModel> MapToAthleteSummaryDbModel(List<AthleteSummary> athleteSummaries)
        {
            return athleteSummaries.Select(summary => new AthleteSummaryDbModel()
                {
                    AthleteName = summary.AthleteName,
                    AvgPace = summary.AveragePace,
                    DistanceSumInKilometers = summary.Distance,
                    ElevationInMeters = summary.ElevationSum,
                    TrainingCount = summary.TrainingCount
                })
                .ToList();
        }
    }
}

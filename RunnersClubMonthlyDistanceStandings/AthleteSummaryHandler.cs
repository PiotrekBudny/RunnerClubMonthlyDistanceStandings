using System;
using System.Collections.Generic;
using System.Linq;
using RunnersClubMonthlyDistanceStandings.Models;

namespace RunnersClubMonthlyDistanceStandings
{
    public class AthleteSummaryHandler
    {
        private readonly List<ActivityDetails> _recordedActivitiesThisMonth;
        private readonly List<AthleteSummary> _athleteSummaries = new List<AthleteSummary>();


        public AthleteSummaryHandler(List<ActivityDetails> recordedActivitiesThisMonth)
        {
            _recordedActivitiesThisMonth = recordedActivitiesThisMonth;
        }

        public void PrepareSummariesFromRecordedActivities()
        {
            var listOfAthletes = _recordedActivitiesThisMonth.Select(x => x.AthleteName).Distinct();

            foreach (var athleteName in listOfAthletes)
            {
                var athleteActivities = _recordedActivitiesThisMonth.Where(x => x.AthleteName == athleteName).ToList();
                var trainingPaceSum = new TimeSpan();

                trainingPaceSum = athleteActivities.Aggregate(trainingPaceSum, (current, activities) => current.Add(activities.TrainingPace));

                var averagePace = (trainingPaceSum / athleteActivities.Count).TrimMilisecondsAndSeconds().ToString("g");

                _athleteSummaries.Add(new AthleteSummary()
                {
                    AthleteName = athleteName,
                    Distance = athleteActivities.Sum(x => x.TrainingDistance),
                    ElevationSum = athleteActivities.Sum(x => x.TrainingElevation),
                    AveragePace = averagePace,
                    TrainingType = athleteActivities.FirstOrDefault()?.TrainingType,
                    TrainingCount =  athleteActivities.Count,
                    IsEmployee = true
                });
            }
        }

        public List<AthleteSummary> GetAthleteSummaries() => _athleteSummaries;
    }
}
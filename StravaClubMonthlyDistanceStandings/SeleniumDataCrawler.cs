using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleTables.Core;
using OpenQA.Selenium;
using StravaClubMonthlyDistanceStandings.Database;
using StravaClubMonthlyDistanceStandings.Database.DbModels;
using StravaClubMonthlyDistanceStandings.Models;
using StravaClubMonthlyDistanceStandings.Pages;

namespace StravaClubMonthlyDistanceStandings
{
    public class SeleniumDataCrawler
    {
        private readonly IWebDriver _webDriver;
        private readonly ConfigurationWrapper _configurationWrapper;
        private readonly WebDriverHandler _driverHandler;
        private List<string> _athleteProfileUrls;
        private readonly List<string> _athletesActivitiesUrls = new List<string>();
        private string _currentActivityType, _currentAthleteActivityProfileUrl;
        private readonly List<ActivityDetails> _recordedActivitiesThisMonth = new List<ActivityDetails>();
        private List<AthleteSummary> _athleteSummaries;
        
        public SeleniumDataCrawler()
        {
            _driverHandler = new WebDriverHandler();
            _webDriver = _driverHandler.InitializeWebDriver();
            _configurationWrapper = new ConfigurationWrapper();
        }

        public void RunSteps()
        {
            Console.WriteLine("Opening Strava in Browser");

            GoToStravaAndFetchData();
            PrepareAthleteSummaries();
            PrintSummariesToConsole();
            SaveAthleteSummariesInDatabase();
        }

        private void GoToStravaAndFetchData()
        {
            try
            {
                var clubPageUrl = StravaUrlBuilder.ClubPage(_configurationWrapper.GetStravaAddressEndpoint(),
                    _configurationWrapper.GetStravaClubId());

                var steps = new LoginPage(_webDriver)
                    .ClickOnLoginLink()
                    .FillInEmailBox(_configurationWrapper.GetStravaUser())
                    .FillInPasswordBox(_configurationWrapper.GetStravaPassword())
                    .ClickOnLoginButton()
                    .MoveToClubPage(clubPageUrl)
                    .ClickOnMembersTab()
                    .MakeSnapshot(MakeSnapshotOfAthleteProfileUrls);
               
                
                Console.WriteLine("Getting feed activities.");
                GetAthletesFeedActivities();
                Console.WriteLine("Getting activities data.");
                GetActivitiesData();
            }
            catch (Exception e)
            {
                Console.WriteLine("We have encountered an error during data gathering: {0}", e.Message);
            }
            finally
            {
                _driverHandler.DisposeWebDriver();
            }
        }

        private void PrepareAthleteSummaries()
        {
            if (_recordedActivitiesThisMonth.Count > 0)
            {
                var summaryHandler = new AthleteSummaryHandler(_recordedActivitiesThisMonth);
                summaryHandler.PrepareSummariesFromRecordedActivities();
                _athleteSummaries = summaryHandler.GetAthleteSummaries();
            }
            else
            {
                Console.WriteLine("No club member has recorded activities.");
            }
        }

        private void SaveAthleteSummariesInDatabase()
        {
            using (var dbContext = new SqLiteDbContext(_configurationWrapper.GetDatabaseConnectionString()))
            {
                var dbHandler = new DbHandler(dbContext);
                var athleteSummariesMapper = new AthleteSummariesMapper();

                if (_athleteSummaries.Count > 0)
                    dbHandler.InsertMonthlySummaryToDatabase(PrepareMonthlySummaryData(),
                        athleteSummariesMapper.MapToAthleteSummaryDbModel(_athleteSummaries));
                else
                {
                    Console.WriteLine("No AthleteSummaries were prepared for saving.");
                }
            }
        }

        private MonthlySummaryDbModel PrepareMonthlySummaryData()
        {
            return new MonthlySummaryDbModel()
            {
                CreatedOn = DateTime.Now.ToString("s"),
                SummaryCode = _configurationWrapper.GetMonthlyFilterValue(),
                TrainingTypes = _configurationWrapper.GetActivityTypes(),
            };
        }

        private void PrintSummariesToConsole()
        {
            var consoleTable =
                new ConsoleTable("AthleteName", "Distance[km]", "ElevationSum[m]", "Avg.Pace[min/km]", "TrainingCount");

            _athleteSummaries = _athleteSummaries.OrderByDescending(x => x.Distance).ToList();

            foreach (var athleteSummary in _athleteSummaries)
            {
                consoleTable.AddRow(athleteSummary.AthleteName, athleteSummary.Distance, athleteSummary.ElevationSum, athleteSummary.AveragePace, athleteSummary.TrainingCount);
            }

            consoleTable.Write();
        }

        private void GetAthletesFeedActivities()
        {
            foreach (var url in _athleteProfileUrls)
            {
                MoveToAthleteMonthlyActivitiesProfilePage(url);

                var getAthleteActivitiesSteps = new AthleteProfilePage(_webDriver)
                    .MakeSnapshot(MakeSnapshotOfAthleteFeedActivitiesUrls);
                Console.WriteLine("Getting activities for athlete of url:{0}", url);

            }
        }

        private void GetActivitiesData()
        {
            foreach (var activityUrl in _athletesActivitiesUrls)
            {
                try
                {
                    MoveToAthleteActivityPage(activityUrl);

                    var getActivityDataSteps = new ActivityPage(_webDriver)
                        .MakeSnapshot(MakeSnapshotActivityType)
                        .MakeSnapshot(MakeSnapshotAthleteUrl);

                    if (_configurationWrapper.GetActivityTypes().Contains(_currentActivityType) && _athleteProfileUrls.Contains(_currentAthleteActivityProfileUrl))
                    {
                        getActivityDataSteps.MakeSnapshot(MakeSnapshotOfActivityData);
                        Console.WriteLine("Got activity data for :{0}", activityUrl);

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Activity not counted towards summary. Unable to gather data from activity {activityUrl}: {e.Message}, {e.StackTrace} {e.InnerException?.Message} {e.InnerException?.StackTrace}");
                }
            }
        }

        private void MoveToAthleteMonthlyActivitiesProfilePage(string profileUrl)
        {
            var urlToGoTo = StravaUrlBuilder.MonthlyActivities(profileUrl, _configurationWrapper.GetMonthlyFilterValue());
            
            _webDriver.Navigate().GoToUrl(urlToGoTo);
        }

        private void MoveToAthleteActivityPage(string activityUrl)
        {
            _webDriver.Navigate().GoToUrl(activityUrl);
        }


        private void MakeSnapshotOfAthleteProfileUrls(ClubPage page)
        {
            _athleteProfileUrls = page.GetAllAthletesProfileUrls();
        }

        private void MakeSnapshotActivityType(ActivityPage page)
        {
            _currentActivityType = page.GetActivityLabelText.Split("– ",2)[1];
        }

        private void MakeSnapshotAthleteUrl(ActivityPage page)
        {
            _currentAthleteActivityProfileUrl = page.GetActivityAthleteProfileHref;
        }

        private void MakeSnapshotOfActivityData(ActivityPage page)
        {
            _recordedActivitiesThisMonth.Add(page.GetActivityDetails());
        }

        private void MakeSnapshotOfAthleteFeedActivitiesUrls(AthleteProfilePage page)
        {
            _athletesActivitiesUrls.AddRange(page.GetAllAthleteActivitiesOfChosenMonth());
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleTables.Core;
using OpenQA.Selenium;
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
            var crawler =
                GoToStravaAndFetchData()
                    .PrepareAthleteSummaries()
                    .PrintSummariesToConsole();

            _driverHandler.DisposeWebDriver();
        }

        private SeleniumDataCrawler GoToStravaAndFetchData()
        {
            var steps = new LoginPage(_webDriver)
                .ClickOnLoginLink()
                .FillInEmailBox(_configurationWrapper.GetStravaUser())
                .FillInPasswordBox(_configurationWrapper.GetStravaPassword())
                .ClickOnLoginButton()
                .MoveToClubPage(BuildClubPageUrl())
                .ClickOnMembersTab()
                .MakeSnapshot(MakeSnapshotOfAthleteProfileUrls);

            GetAthletesFeedActivities();
            GetActivitiesData();

            return this;
        }

        private SeleniumDataCrawler PrepareAthleteSummaries()
        {
            var summaryHandler = new AthleteSummaryHandler(_recordedActivitiesThisMonth);
            summaryHandler.PrepareSummariesFromRecordedActivities();
            _athleteSummaries = summaryHandler.GetAthleteSummaries();
            
            return this;
        }

        private SeleniumDataCrawler PrintSummariesToConsole()
        {
            Console.WriteLine();
            Console.WriteLine(DateTime.Now);
            Console.WriteLine("AthleteName, Distance[km], ElevationSum[m], Avg.Pace[min/km], TrainingCount");

            var consoleTable =
                new ConsoleTable("AthleteName", "Distance[km]", "ElevationSum[m]", "Avg.Pace[min/km]", "TrainingCount");

            _athleteSummaries = _athleteSummaries.OrderByDescending(x => x.Distance).ToList();

            foreach (var athleteSummary in _athleteSummaries)
            {
                consoleTable.AddRow(athleteSummary.AthleteName, athleteSummary.Distance, athleteSummary.ElevationSum, athleteSummary.AveragePace, athleteSummary.TrainingCount);
            }

            consoleTable.Write();

            return this;
        }

        private string BuildClubPageUrl() => string.Concat(_configurationWrapper.GetStravaAddressEndpoint(), "/clubs/",
            _configurationWrapper.GetStravaClubId());

        private string BuildMonthlyActivitiesUrl(string profileUrl) =>
            string.Concat(profileUrl,
                "#interval?interval=",
                _configurationWrapper.GetMonthlyFilterValue(),
                "&interval_type=month&chart_type=miles&year_offset=0");

        private void GetAthletesFeedActivities()
        {
            foreach (var url in _athleteProfileUrls)
            {
                MoveToAthleteMonthlyActivitiesProfilePage(url);

                var getAthleteActivitiesSteps = new AthleteProfilePage(_webDriver)
                    .MakeSnapshot(MakeSnapshotOfAthleteFeedActivitiesUrls);
            }
        }

        private void GetActivitiesData()
        {
            foreach (var activityUrl in _athletesActivitiesUrls)
            {
                MoveToAthleteActivityPage(activityUrl);
                Console.WriteLine(activityUrl);

                var getActivityDataSteps = new ActivityPage(_webDriver)
                    .MakeSnapshot(MakeSnapshotActivityType)
                    .MakeSnapshot(MakeSnapshotAthleteUrl);

                if (_configurationWrapper.GetActivityTypes().Contains(_currentActivityType) && _athleteProfileUrls.Contains(_currentAthleteActivityProfileUrl))
                {
                    getActivityDataSteps.MakeSnapshot(MakeSnapshotOfActivityData);
                }
            }
        }

        private void MoveToAthleteMonthlyActivitiesProfilePage(string profileUrl)
        {
            var urlToGoTo = BuildMonthlyActivitiesUrl(profileUrl);
            
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
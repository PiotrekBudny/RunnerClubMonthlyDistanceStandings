using System;
using System.Collections.Generic;
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
        private string _currentActivityType;
        private readonly List<ActivityDetails> _recordedActivitiesThisMonth = new List<ActivityDetails>();
        
        public SeleniumDataCrawler()
        {
            _driverHandler = new WebDriverHandler();
            _webDriver = _driverHandler.InitializeWebDriver();
            _configurationWrapper = new ConfigurationWrapper();
        }


        public void RunSteps()
        {
            GoToStravaAndFetchData();
            
            Console.ReadLine();

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

            GetAthletesActivities();
            GetActivitiesData();

            return this;
        }

        private string BuildClubPageUrl() => string.Concat(_configurationWrapper.GetStravaAddressEndpoint(), "/clubs/",
            _configurationWrapper.GetStravaClubId());

        private string BuildMonthlyActivitiesUrl(string profileUrl) =>
            string.Concat(profileUrl,
                "#interval?interval=",
                _configurationWrapper.GetMonthlyFilterValue(),
                "&interval_type=month&chart_type=miles&year_offset=0");

        private void GetAthletesActivities()
        {
            foreach (var url in _athleteProfileUrls)
            {
                Console.WriteLine(url);
                MoveToAthleteMonthlyActivitiesProfilePage(url);

                var getAthleteActivitiesSteps = new AthleteProfilePage(_webDriver)
                    .MakeSnapshot(MakeSnapshotOfAthleteActivitiesUrls);
            }
        }

        private void GetActivitiesData()
        {
            foreach (var activityUrl in _athletesActivitiesUrls)
            {
                MoveToAthleteActivityPage(activityUrl);
                Console.WriteLine(activityUrl);

                var getActivityDataSteps = new ActivityPage(_webDriver)
                    .MakeSnapshot(MakeSnapshotActivityType);

                if (_currentActivityType.Contains(_configurationWrapper.GetActivityType()))
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
            _currentActivityType = page.GetActivityLabelText;
        }

        private void MakeSnapshotOfActivityData(ActivityPage page)
        {
            _recordedActivitiesThisMonth.Add(page.GetActivityDetails());
        }

        private void MakeSnapshotOfAthleteActivitiesUrls(AthleteProfilePage page)
        {
            _athletesActivitiesUrls.AddRange(page.GetAllAthleteActivitiesOfChosenMonth());
        }
    }
}
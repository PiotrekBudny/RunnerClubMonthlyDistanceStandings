using System;
using System.Collections.Generic;
using OpenQA.Selenium;
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

            foreach (var url in _athleteProfileUrls)
            {
                Console.WriteLine(url);
                MoveToAthleteMonthlyActivitiesProfilePage(url);

                var getAthleteActivitesSteps = new AthleteProfilePage(_webDriver)
                    .MakeSnapshot(MakeSnapshotOfAthleteActivitiesUrls);
            }

            foreach (var activity in _athletesActivitiesUrls)
            {
                Console.WriteLine(activity);
            }

            return this;
        }

        private string BuildClubPageUrl() => string.Concat(_configurationWrapper.GetStravaAddressEndpoint(), "/clubs/",
            _configurationWrapper.GetStravaClubId());

        private string BuildMonthlyActivitiesUrl(string profileUrl) =>
            string.Concat(profileUrl,
                "#interval?interval=",
                _configurationWrapper.GetMonthlyFilterValue(),
                "&interval_type=month&chart_type=miles&year_offset=0");

        private void MoveToAthleteMonthlyActivitiesProfilePage(string profileUrl)
        {
            var urlToGoTo = BuildMonthlyActivitiesUrl(profileUrl);
            
            _webDriver.Navigate().GoToUrl(urlToGoTo);
        }


        private void MakeSnapshotOfAthleteProfileUrls(ClubPage screen)
        {
            _athleteProfileUrls = screen.GetAllAthletesProfileUrls();
        }

        private void MakeSnapshotOfAthleteActivitiesUrls(AthleteProfilePage screen)
        {
            _athletesActivitiesUrls.AddRange(screen.GetAllAthleteActivitiesOfChosenMonth());
        }
    }
}
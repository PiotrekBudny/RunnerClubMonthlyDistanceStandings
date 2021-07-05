using System;
using OpenQA.Selenium;
using StravaClubMonthlyDistanceStandings.Models;

namespace StravaClubMonthlyDistanceStandings.Pages
{
    public class ActivityPage : Page
    {
        private IWebElement ElevationValue =>
            WebDriver.FindElement(By.XPath("//*[@id='heading']/div/div[1]/div[2]/div[1]/div[1]/div[2]/strong"));

        private IWebElement AthleteNameLink => WebDriver.FindElement(By.XPath("//*[@id='heading']/header/h2/span/a"));

        private IWebElement ActivityType => WebDriver.FindElement(By.XPath("//*[@id='heading']/header/h2/span"));

        private IWebElement Distance =>
            WebDriver.FindElement(By.XPath("//*[@id='heading']/div/div[1]/div[2]/ul/li[1]/strong"));

        private IWebElement Pace =>
            WebDriver.FindElement(By.XPath("//*[@id='heading']/div/div[1]/div[2]/ul/li[3]/strong"));


        public ActivityPage(IWebDriver webDriver)
        {
            WebDriver = webDriver;
        }

        public ActivityPage MakeSnapshot(Action<ActivityPage> action)
        {
            action(this);

            return this;
        }

        public string GetActivityLabelText => ActivityType.Text;

        public ActivityDetails GetActivityDetails()
        {
            var distance = Distance.Text.RemoveMeasureUnitFromString();

            var pace = Pace.Text.RemoveMeasureUnitFromString();

            var athleteName = AthleteNameLink.Text;

            var activityType = ActivityType.Text.GetActivityTypeSubstring();

            var elevationValue = ElevationValue.Text.RemoveMeasureUnitFromString();

            return  new ActivityDetails()
            {
                AthleteName = athleteName,
                TrainingDistance = distance,
                TrainingPace = pace,
                TrainingElevation = elevationValue,
                TrainingType = activityType
            };
        }
    }
}
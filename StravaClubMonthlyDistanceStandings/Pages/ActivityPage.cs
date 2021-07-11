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

        private IWebElement AthleteProfileUrl => WebDriver.FindElement(By.XPath("//*[@id='heading']/header/h2/span/a"));

        public ActivityPage(IWebDriver webDriver) : base()
        {
            WebDriver = webDriver;
        }

        public ActivityPage MakeSnapshot(Action<ActivityPage> action)
        {
            action(this);

            return this;
        }

        public string GetActivityLabelText => ActivityType.Text;
        public string GetActivityAthleteProfileHref => AthleteProfileUrl.GetAttribute("Href");


        public ActivityDetails GetActivityDetails()
        {
            return  new ActivityDetails()
            {
                AthleteName = AthleteNameLink.Text.GetAthleteNameShortened(),
                TrainingDistance = decimal.Parse(Distance.Text.RemoveMeasureUnitFromString()),
                TrainingPace = TimeSpan.Parse(Pace.Text.RemoveMeasureUnitFromString()),
                TrainingElevation = decimal.Parse(ElevationValue.Text.RemoveMeasureUnitFromString()),
                TrainingType = ActivityType.Text.GetActivityTypeSubstring()
            };
        }
    }
}
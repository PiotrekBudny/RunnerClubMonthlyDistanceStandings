using System;
using System.Globalization;
using System.Threading;
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
        public string GetActivityAthleteProfileHref => AthleteProfileUrl.GetAttribute("Href");


        public ActivityDetails GetActivityDetails()
        {
            string elevation;

            try
            {
                elevation = ElevationValue.Text.RemoveMeasureUnitFromString();
            }
            catch (Exception e)
            {
                elevation = "0";
            }

            return  new ActivityDetails()
            {
                AthleteName = AthleteNameLink.Text.GetAthleteNameShortened(),
                TrainingDistance = decimal.Parse(Distance.Text.RemoveMeasureUnitFromString(), CultureInfo.InvariantCulture),
                TrainingPace = TimeSpan.Parse(Pace.Text.RemoveMeasureUnitFromString()),
                TrainingElevation = decimal.Parse(elevation, CultureInfo.InvariantCulture),
                TrainingType = ActivityType.Text.GetActivityTypeSubstring()
            };
        }
    }
}
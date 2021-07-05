using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace StravaClubMonthlyDistanceStandings.Pages
{
    public class AthleteProfilePage : Page
    {
        public AthleteProfilePage(IWebDriver webWebDriver)
        {
            WebDriver = webWebDriver;
        }

        public AthleteProfilePage MakeSnapshot(Action<AthleteProfilePage> action)
        {
            action(this);
            
            return this;
        }

        public List<string> GetAllAthleteActivitiesOfChosenMonth()
        {
            ScrollPageDownToLoadAllData();

            var athleteActivitiesUrls = new List<string>();
            
            try
            {
                var feedElements = WebDriver.FindElements(By.ClassName("entry-body"));
                
                if (feedElements.Count == 0)
                    throw new Exception();

                foreach (var element in feedElements)
                {
                    var childElements = element.FindElement(By.TagName("a"));

                    athleteActivitiesUrls.Add(childElements.GetAttribute("href"));
                }
            }
            catch (Exception)
            {
                Console.WriteLine("No Activities found");
            }


            return athleteActivitiesUrls;
        }
    }
}
using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace RunnersClubMonthlyDistanceStandings.Pages
{
    public class ClubPage : Page
    {
        private IWebElement MembersTab => WebDriver.FindElement(By.LinkText("Members"));

        public ClubPage(IWebDriver webWebDriver)
        {
            WebDriver = webWebDriver;
        }

        public ClubPage ClickOnMembersTab()
        {
            MembersTab.Click();
            return this;
        }

        public ClubPage MakeSnapshot(Action<ClubPage> action)
        {
            action(this);
            return this;
        }

        public List<string> GetAllAthletesProfileUrls()
        {
            ScrollPageDownToLoadAllData();

            var athleteProfileUrls = new List<string>();

            try
            {
                var elements = WebDriver.FindElements(By.ClassName("text-headline"));

                foreach (var element in elements)
                {
                    var childElement = element.FindElement(By.XPath(".//*"));

                    athleteProfileUrls.Add(childElement.GetAttribute("href"));
                }
            }
            catch (Exception)
            {
                throw new Exception("No club members found, tool will be stopped");
            }

            return athleteProfileUrls;
        }
    }
}
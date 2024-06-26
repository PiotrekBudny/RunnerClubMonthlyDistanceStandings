﻿using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace RunnersClubMonthlyDistanceStandings.Pages
{
    public class AthleteProfilePage : Page
    {
        public AthleteProfilePage(IWebDriver webWebDriver)
        {
            WebDriver = webWebDriver;
            ScrollPageDownToLoadAllData();
        }

        public AthleteProfilePage MakeSnapshot(Action<AthleteProfilePage> action)
        {
            action(this);
            
            return this;
        }

        public List<string> GetAllAthleteActivitiesOfChosenMonth()
        {
            var athleteActivitiesUrls = new List<string>();
            
            try
            {
                var feedElements = WebDriver.FindElements(By.ClassName("entry-body"));

                if (feedElements.Count == 0)
                    throw new Exception();

                foreach (var element in feedElements)
                {
                    var childElements = element.FindElements(By.TagName("a"));

                    foreach (var child in childElements)
                    {
                        var href = child.GetAttribute("Href");

                        if (athleteActivitiesUrls.ValidateIfUrlIsAlreadyOnList(href) == false && href.ValidateIfActivityUrlIsMainActivityLink())
                            athleteActivitiesUrls.Add(href);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"No Activities found: {e.Message} {e.InnerException?.Message} {e.StackTrace} {e.InnerException?.StackTrace}");
            }


            return athleteActivitiesUrls;
        }
    }
}
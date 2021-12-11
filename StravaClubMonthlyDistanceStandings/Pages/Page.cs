using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace StravaClubMonthlyDistanceStandings.Pages
{
    public class Page
    {
        protected IWebDriver WebDriver;

        public Page()
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
        }

        protected void ScrollPageDownToLoadAllData()
        {
            var javaScriptExecutor = (IJavaScriptExecutor) WebDriver;
            var lastHeight = javaScriptExecutor.ExecuteScript("return document.body.scrollHeight");
            
            while (true)
            {
                javaScriptExecutor.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");

                Thread.Sleep(TimeSpan.FromSeconds(5));
                var newHeight = javaScriptExecutor.ExecuteScript("return document.body.scrollHeight");

                if (newHeight.Equals(lastHeight))
                    break;

                lastHeight = newHeight;
            }
        }
    }
}
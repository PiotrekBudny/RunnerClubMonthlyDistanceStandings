using System;
using System.Threading;
using OpenQA.Selenium;

namespace StravaClubMonthlyDistanceStandings.Pages
{
    public class Page
    {
        protected IWebDriver WebDriver;

        protected void ScrollPageDownToLoadAllData()
        {
            var javaScriptExecutor = (IJavaScriptExecutor) WebDriver;
            var lastHeight = javaScriptExecutor.ExecuteScript("return document.body.scrollHeight");
            
            while (true)
            {
                javaScriptExecutor.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");

                Thread.Sleep(TimeSpan.FromSeconds(2));
                var newHeight = javaScriptExecutor.ExecuteScript("return document.body.scrollHeight");

                if (newHeight.Equals(lastHeight))
                    break;

                lastHeight = newHeight;
            }
        }
    }
}
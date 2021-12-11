using System;

namespace StravaClubMonthlyDistanceStandings
{
    class Program
    {
        static void Main(string[] args)
        {
            var crawler = new SeleniumDataCrawler();
            crawler.RunSteps();

            Environment.Exit(0);
        }
    }
}

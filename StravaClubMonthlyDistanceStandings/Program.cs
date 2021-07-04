using System;

namespace StravaClubMonthlyDistanceStandings
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Opening Strava in Browser");

            var crawler = new SeleniumDataCrawler();
            crawler.RunSteps();

            Console.ReadLine();
        }
    }
}

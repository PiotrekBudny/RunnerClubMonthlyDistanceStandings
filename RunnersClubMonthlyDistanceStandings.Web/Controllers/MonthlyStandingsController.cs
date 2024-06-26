﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RunnersClubMonthlyDistanceStandings.Database;

namespace RunnersClubMonthlyDistanceStandings.Web.Controllers
{
    public class MonthlyStandingsController : Controller
    {
        private readonly ILogger<MonthlyStandingsController> _logger;
        private readonly ConfigurationWrapper _configurationWrapper;
        public MonthlyStandingsController(ILogger<MonthlyStandingsController> logger)
        {
            _logger = logger;
            _configurationWrapper = new ConfigurationWrapper();
        }


        public IActionResult GetLatestMonthlyStandingsBySummaryCode()
        {
            var databaseDataProvider = new DatabaseDataProvider(new SqLiteDbContext(_configurationWrapper.GetDatabaseConnectionString()));

            var response = databaseDataProvider.GetLatestMonthlyReport(_configurationWrapper.GetMonthlyFilterValue());

            return View(response);
        }
    }
}

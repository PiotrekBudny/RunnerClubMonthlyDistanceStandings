using Microsoft.Extensions.Configuration;

namespace StravaClubMonthlyDistanceStandings.Web
{
 public class ConfigurationWrapper
    {
        public string GetDatabaseConnectionString()
        {
            return InitializeConfigurationBuilder().Build()["SQLiteConnectionStringPath"];
        }

        public string GetMonthlyFilterValue()
        {
            var builder = InitializeConfigurationBuilder().Build();

            var year = builder["Data:TrainingYear"];
            var month = builder["Data:TrainingMonth"];

            return string.Concat(year, month);
        }

        private IConfigurationBuilder InitializeConfigurationBuilder()
        {
            return new ConfigurationBuilder().AddJsonFile("appsettings.json");
        }
    }
}

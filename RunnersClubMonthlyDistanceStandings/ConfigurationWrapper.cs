using Microsoft.Extensions.Configuration;

namespace RunnersClubMonthlyDistanceStandings
{
    public class ConfigurationWrapper
    {
        public string GetAddressEndpoint()
        {
            return InitializeConfigurationBuilder().Build()["Settings:Endpoint"];            
        }

        public string GetUser()
        {
            return InitializeConfigurationBuilder().Build()["Settings:Username"];
        }

        public string GetPassword()
        {
            return InitializeConfigurationBuilder().Build()["Settings:UserPassword"];
        }

        public string GetClubId()
        {
            return InitializeConfigurationBuilder().Build()["Settings:ClubId"];
        }

        public string GetMonthlyFilterValue()
        {
            var builder = InitializeConfigurationBuilder().Build();

            var year = builder["Data:TrainingYear"];
            var month = builder["Data:TrainingMonth"];

            return string.Concat(year, month);
        }

        public string GetActivityTypes()
        {
            return InitializeConfigurationBuilder().Build()["Data:TrainingTypes"];
        }
        public string GetDatabaseConnectionString()
        {
            return InitializeConfigurationBuilder().Build()["SQLiteConnectionStringPath"];
        }

        private IConfigurationBuilder InitializeConfigurationBuilder()
        {
            return new ConfigurationBuilder().AddJsonFile("appsettings.json");
        }   
    }
}

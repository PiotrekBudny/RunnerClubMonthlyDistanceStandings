using Microsoft.Extensions.Configuration;

namespace StravaClubMonthlyDistanceStandings
{
    public class ConfigurationWrapper
    {
        public string GetStravaAddressEndpoint()
        {
            return InitializeConfigurationBuilder().Build()["StravaSettings:Endpoint"];            
        }

        public string GetStravaUser()
        {
            return InitializeConfigurationBuilder().Build()["StravaSettings:Username"];
        }

        public string GetStravaPassword()
        {
            return InitializeConfigurationBuilder().Build()["StravaSettings:UserPassword"];
        }

        public string GetStravaClubId()
        {
            return InitializeConfigurationBuilder().Build()["StravaSettings:ClubId"];
        }

        public string GetMonthlyFilterValue()
        {
            var builder = InitializeConfigurationBuilder().Build();

            var year = builder["Data:TrainingYear"];
            var month = builder["Data:TrainingMonth"];

            return string.Concat(year, month);
        }

        public string GetActivityType()
        {
            return InitializeConfigurationBuilder().Build()["Data:TrainingType"];
        }
        
        private IConfigurationBuilder InitializeConfigurationBuilder()
        {
            return new ConfigurationBuilder().AddJsonFile("appsettings.json");
        }   
    }
}

using Microsoft.Edge.SeleniumTools;
using OpenQA.Selenium;

namespace StravaClubMonthlyDistanceStandings
{
    public class WebDriverHandler
    {
        private IWebDriver _webDriver;

        public IWebDriver InitializeWebDriver()
        {
            var configuration = new ConfigurationWrapper();

            var options = new EdgeOptions {UseChromium = true};
            options.AddArgument("--silent");
            options.AddArgument("log-level=3");

            _webDriver = new EdgeDriver(options);
            _webDriver.Manage().Window.Maximize();

            _webDriver.Navigate().GoToUrl(configuration.GetStravaAddressEndpoint());

            return _webDriver;
        }

        public void DisposeWebDriver()
        {
            _webDriver.Close();
            _webDriver.Dispose();
        }
    }
}
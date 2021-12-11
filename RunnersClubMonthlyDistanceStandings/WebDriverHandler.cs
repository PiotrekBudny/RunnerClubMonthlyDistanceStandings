using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace RunnersClubMonthlyDistanceStandings
{
    public class WebDriverHandler
    {
        private IWebDriver _webDriver;

        public IWebDriver InitializeWebDriver()
        {
            var configuration = new ConfigurationWrapper();

            var options = new ChromeOptions();
            options.AddArgument("--silent");
            options.AddArgument("log-level=3");
            options.AddArgument("headless"); 
            options.AddArgument("--no-sandbox");
            //options.AddArgument("--disable-software-rasterizer");

            _webDriver = new ChromeDriver(options);
            _webDriver.Manage().Window.Maximize();

            _webDriver.Navigate().GoToUrl(configuration.GetAddressEndpoint());

            return _webDriver;
        }

        public void DisposeWebDriver()
        {
            _webDriver.Close();
            _webDriver.Dispose();
        }
    }
}
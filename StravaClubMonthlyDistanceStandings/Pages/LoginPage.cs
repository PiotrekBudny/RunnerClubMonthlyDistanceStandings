using OpenQA.Selenium;

namespace StravaClubMonthlyDistanceStandings.Pages
{
    public class LoginPage : Page
    {
        private IWebElement Email => WebDriver.FindElement(By.Id("email"));
        private IWebElement Password => WebDriver.FindElement(By.Id("password"));
        private IWebElement LoginButton => WebDriver.FindElement(By.Id("login-button"));
        private IWebElement LoginLink => WebDriver.FindElement(By.LinkText("Log In"));


        public LoginPage(IWebDriver webWebDriver)
        {
            WebDriver = webWebDriver;
        }

        public LoginPage ClickOnLoginLink()
        {
            LoginLink.Click();

            return this;
        }

        public LoginPage FillInEmailBox(string email)
        {
            Email.SendKeys(email);

            return this;
        }

        public LoginPage FillInPasswordBox(string password)
        {
            Password.SendKeys(password);

            return this;
        }

        public LoginPage ClickOnLoginButton()
        {
            LoginButton.Click();

            return this;
        }

        public ClubPage MoveToClubPage(string clubPageUrl)
        {
            WebDriver.Navigate().GoToUrl(clubPageUrl);

            return new ClubPage(WebDriver);
        }

    }
}
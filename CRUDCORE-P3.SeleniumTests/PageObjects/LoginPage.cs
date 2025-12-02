using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CRUDCORE_P3.SeleniumTests.PageObjects
{
    public class LoginPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        private By correoInput = By.Name("correo");
        private By passwordInput = By.Name("password");
        private By loginButton = By.CssSelector("button.btn.btn-primary");
        private By errorMessage = By.CssSelector("p.text-danger");

        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void NavigateTo(string url)
        {
            driver.Navigate().GoToUrl(url);
        }

        public void EnterCorreo(string correo)
        {
            wait.Until(d => d.FindElement(correoInput)).Clear();
            driver.FindElement(correoInput).SendKeys(correo);
        }

        public void EnterPassword(string password)
        {
            driver.FindElement(passwordInput).Clear();
            driver.FindElement(passwordInput).SendKeys(password);
        }

        public void ClickLogin()
        {
            driver.FindElement(loginButton).Click();
        }

        public void Login(string correo, string password)
        {
            EnterCorreo(correo);
            EnterPassword(password);
            ClickLogin();
        }

        public bool IsErrorMessageDisplayed()
        {
            try
            {
                return driver.FindElement(errorMessage).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public string GetErrorMessage()
        {
            return driver.FindElement(errorMessage).Text;
        }
    }
}
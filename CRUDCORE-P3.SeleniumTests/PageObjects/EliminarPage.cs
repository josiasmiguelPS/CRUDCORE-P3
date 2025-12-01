using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CRUDCORE_P3.SeleniumTests.PageObjects
{
    public class EliminarPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;

        private By eliminarButton = By.CssSelector("button.btn.btn-danger");
        private By volverButton = By.LinkText("Volvers");
        private By nombreDisplay = By.Id("NombreCompleto");

        public EliminarPage(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public void ClickEliminar()
        {
            wait.Until(d => d.FindElement(eliminarButton)).Click();
        }

        public string GetNombreDisplayed()
        {
            return driver.FindElement(nombreDisplay).GetAttribute("value");
        }
    }
}
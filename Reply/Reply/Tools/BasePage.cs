using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Diagnostics;

namespace Reply.Tools

{
    public class BasePage
    {
        protected IWebDriver driver = null;
        public static readonly By ListLocator = By.XPath("//span[@class='detailLink']//a[@class='listViewNameLink']");

        protected BasePage(IWebDriver webDriver, string url)
        {
            this.driver = webDriver;

            try
            {
                Wait(driver).Until(ExpectedConditions.UrlContains(url));
                Wait(driver).Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
            }
            catch (Exception e)
            { Debug.WriteLine("Wrong URL or Page not ready"); }
            Thread.Sleep(1000);
        }

        public BasePage()
        {
        }

        public IWebElement Find(By locator)
        {
            return driver.FindElement(locator);
        }

        public IList<IWebElement> FindMultiple(By locator)
        {
            return driver.FindElements(locator).ToList();
        }

        public IWebDriver GetWebDriver(string browser)
        {
            switch (browser)
            {
                case "Chrome":
                    driver = new ChromeDriver(new ChromeOptions());
                    break;

                case "Firefox":
                    driver = new FirefoxDriver(new FirefoxOptions());
                    break;

                case "Edge":
                    driver = new EdgeDriver(new EdgeOptions());
                    break;
            }

            driver.Manage().Window.Maximize();
            return driver;
        }

        public WebDriverWait Wait(IWebDriver driver)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
    }
}
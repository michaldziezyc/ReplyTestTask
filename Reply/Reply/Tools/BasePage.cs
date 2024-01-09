using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Diagnostics;

namespace Reply.Tools

{
    public class BasePage
    {
        protected IWebDriver driver = null;
        public static readonly By ListLocator = By.XPath("//span[@class='detailLink']//a[@class='listViewNameLink']");
        public static IConfigurationRoot settings = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        protected BasePage(IWebDriver webDriver, string url)
        {
            this.driver = webDriver;

            try
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until(ExpectedConditions.UrlContains(url));
                new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
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

        public IConfigurationRoot settingss = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        public IWebDriver GetWebDriver(string browser)
        {
            switch (browser)
            {
                case "Chrome":
                    ChromeOptions chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("start-maximized");

                    driver = new ChromeDriver(chromeOptions);
                    break;

                case "Firefox":
                    FirefoxOptions firefoxOptions = new FirefoxOptions();
                    firefoxOptions.AddArgument("--width=1920");
                    firefoxOptions.AddArgument("--height=1080");

                    driver = new FirefoxDriver(firefoxOptions);

                    break;

                case "Edge":
                    EdgeOptions edgeOptions = new EdgeOptions();
                    edgeOptions.AddArgument("start-maximized");

                    driver = new EdgeDriver(edgeOptions);
                    break;
            }
            return driver;
        }
    }
}
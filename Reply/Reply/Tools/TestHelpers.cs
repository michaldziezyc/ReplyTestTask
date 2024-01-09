using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Reply.Tools
{
    public class TestHelpers
    {
        public static void WaitUntilPageLoaded(string url, IWebDriver driver)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until(ExpectedConditions.UrlContains(url));
            new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
        }
    }
}
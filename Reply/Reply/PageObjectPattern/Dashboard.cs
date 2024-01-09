using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Reply.Tools;

namespace Reply.PageObjectPattern
{
    public class Dashboard : BasePage
    {
        public Dashboard(IWebDriver driver) : base(driver, "index.php?module=Home")
        {
        }

        public void NavigateTo(string tabName, string optionName)
        {
            new Actions(driver).MoveToElement(Find(By.XPath($"//a[@title='{tabName}']"))).Build().Perform();
            Thread.Sleep(1000);
            Find(By.XPath($"//a[@class='menu-tab-sub-list' and text() = ' {optionName}']")).Click();
        }
    }
}
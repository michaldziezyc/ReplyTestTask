using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reply.Tools;
using SeleniumExtras.WaitHelpers;

namespace Reply.PageObjectPattern
{
    public class ActivityList : BasePage
    {
        public ActivityList(IWebDriver driver) : base(driver, "module=ActivityLog&action=index")
        {
        }

        private IWebElement numberOfLogs => Find(By.XPath("//span[contains(@id,'SelectCountHead')]/following-sibling::span"));
        private IList<IWebElement> listOfActivityCheckboxes => FindMultiple(By.XPath("//tr[contains(@class,'listViewRow')]//input")).ToList();
        private IWebElement ActionsDropDown => Find(By.XPath("//button[contains(@id,'ActionButtonHead')]"));

        private By DeleteOptionLocator = By.XPath("//div[contains(@id,'ActionButtonHead')]//div[text()='Delete']");
        private IWebElement DeleteOption => Find(DeleteOptionLocator);

        public void SelectLogs(int numberOfLogs)
        {
            for (int i = 0; i < numberOfLogs; i++)
            {
                listOfActivityCheckboxes[i].Click();
            }
        }

        public void DeleteLogs()
        {
            ActionsDropDown.Click();
            Wait(driver).Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(DeleteOptionLocator));
            DeleteOption.Click();

            driver.SwitchTo().Alert().Accept();
        }

        public int GetNumberOfLogs()
        {
            return Int32.Parse(numberOfLogs.Text.Replace(",", ""));
        }
    }
}
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reply.Tools;

namespace Reply.PageObjectPattern
{
    public class ReportsList : BasePage
    {
        public ReportsList(IWebDriver driver) : base(driver, "module=Reports&action=index")
        {
        }

        private IWebElement filterField => Find(By.Id("filter_text"));
        private IList<IWebElement> listOfReports => FindMultiple(ListLocator).ToList();

        public void SearchReport(string reportName)
        {
            filterField.SendKeys($"{reportName}");
            filterField.SendKeys(Keys.Enter);
            Wait(driver).Until(e => listOfReports.Count != 20);

            Assert.That(listOfReports.Count, Is.EqualTo(1));
            listOfReports.First().Click();
        }
    }
}
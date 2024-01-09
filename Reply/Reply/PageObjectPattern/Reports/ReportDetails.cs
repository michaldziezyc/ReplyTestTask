using NUnit.Framework;
using OpenQA.Selenium;
using Reply.Tools;

namespace Reply.PageObjectPattern
{
    public class ReportDetails : BasePage
    {
        private IWebElement detailForm => Find(By.XPath("//form[@name='FilterForm']"));

        public IWebElement runReportButton => Find(By.XPath("//button[@name='FilterForm_applyButton']"));

        private IList<IWebElement> reportRows => FindMultiple(ListLocator).ToList();

        public ReportDetails(IWebDriver driver, string reportName) : base(driver, "module=Project&action=index&layout=Reports")
        {
            Assert.That(detailForm.Text, Does.Contain(reportName));
        }

        public int CountRowsOnReport()
        {
            return reportRows.Count;
        }
    }
}
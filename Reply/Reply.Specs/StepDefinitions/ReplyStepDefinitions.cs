using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reply.PageObjectPattern;
using Reply.Tools;
using SeleniumExtras.WaitHelpers;

namespace Reply.Specs.StepDefinitions
{
    [Binding]
    public sealed class ReplyStepDefinitions
    {
        private static IConfigurationRoot settings = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        private static IWebDriver driver = new BasePage().GetWebDriver(settings["Browser"]);
        private static string URL = settings["URL"];

        #region Fixtures

        [BeforeTestRun]
        [Given(@"I open login page and log in")]
        public static void GivenIOpenLoginPageAndLogIn()
        {
            driver.Navigate().GoToUrl(URL);

            Login login = new Login(driver);
            login.LogIn(settings["Username"], settings["Password"]);
            new Dashboard(driver);
        }

        [BeforeScenario]
        public static void StartFromDashboard()
        {
            driver.FindElement(By.XPath("//a//div[contains(text(),' Activities')]")).Click();
        }

        [AfterTestRun]
        public static void CloseDriver()
        {
            driver?.Quit();
        }

        #endregion Fixtures

        #region CreateContact

        private string contactFirstName = RandomHelpers.RandomString(8);
        private string contactLastName = RandomHelpers.RandomString(8);
        private string role = "CFO";

        [Given(@"I go to Sales&Marketing->Contacts")]
        public void GivenIGoToSalesMarketing_Contacts()
        {
            new Dashboard(driver).NavigateTo("Sales & Marketing", "Contacts");
        }

        [Given(@"I create new contact")]
        public void ThenICreateNewContact()
        {
            ContactsList contactsList = new ContactsList(driver);
            contactsList.createContact.Click();

            ContactsCreate contactsCreate = new ContactsCreate(driver);
            contactsCreate.FillCustomerInfo(contactFirstName, contactLastName, role);
            contactsCreate.saveButton.Click();
        }

        [When(@"I open latest contact")]
        public void ThenIOpenLatestContact()
        {
            ContactsDetails contactsDetails = new ContactsDetails(driver);
            contactsDetails.contactsShortcut.Click();

            ContactsList contactsList = new ContactsList(driver);
            contactsList.SearchCustomer(contactFirstName, contactLastName);
        }

        [Then(@"I check if data is correct")]
        public void ThenICheckIfDataIsCorrect()
        {
            ContactsDetails contactsDetails = new ContactsDetails(driver);
            contactsDetails.CheckIfCustomerHasExpectedData(
                contactFirstName,
                contactLastName,
                role,
                "Customers, Suppliers");
        }

        #endregion CreateContact

        #region Run report

        private string reportName = "Project Profitability";

        [Given(@"I go to Reports&Settings->Reports")]
        public void GivenIGoToReportsSettings_Reports()
        {
            new Dashboard(driver).NavigateTo("Reports & Settings", "Reports");
        }

        [Given(@"I find Project Profitability report")]
        public void GivenIFindProjectProfitabilityReport()
        {
            ReportsList reportsList = new ReportsList(driver);
            reportsList.SearchReport(reportName);
        }

        [When(@"I run Project Profitability report")]
        public void WhenIRunProjectProfitabilityReport()
        {
            new ReportDetails(driver, reportName).runReportButton.Click();
        }

        [Then(@"I verify that results were returned")]
        public void ThenIVerifyThatResultsWereReturned()
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(BasePage.ListLocator));
            Assert.That(new ReportDetails(driver, "Project Profitability").CountRowsOnReport(), Is.GreaterThan(0));
        }

        #endregion Run report

        #region Remove events from activity log

        private int beforeDeleting;
        private int afterDeleting;

        [Given(@"I go to Reports&Settings->ActivityLog")]
        public void GivenIGoToReportsSettings_ActivityLog()
        {
            new Dashboard(driver).NavigateTo("Reports & Settings", "Activity Log");
        }

        [Given(@"I select first (.*) items in the table")]
        public void GivenISelectFirstItemsInTheTable(int number)
        {
            ActivityList activityList = new ActivityList(driver);

            beforeDeleting = activityList.GetNumberOfLogs();

            activityList.SelectLogs(number);
        }

        [When(@"I delete first (.*) items in the table")]
        public void WhenIDeleteThem(int number)
        {
            ActivityList activityList = new ActivityList(driver);

            activityList.DeleteLogs();
        }

        [Then(@"I verify that items were deleted")]
        public void ThenIVerifyThatItemsWereDeleted()
        {
            ActivityList activityList = new ActivityList(driver);

            afterDeleting = activityList.GetNumberOfLogs();

            Assert.That(afterDeleting, Is.EqualTo(beforeDeleting - 3));
        }

        #endregion Remove events from activity log
    }
}
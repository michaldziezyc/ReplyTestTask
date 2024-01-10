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
        public static void GoToLoginPageAndLogIn()
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
            try
            {
                driver.SwitchTo().Alert().Accept();
            }
            catch (Exception ex) { }
            _ = new Dashboard(driver);
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
        private string role = "Sales";

        [Given(@"a user goes to Sales&Marketing->Contacts")]
        public void GivenAUserGoesToSalesMarketing_Contacts()
        {
            new Dashboard(driver).NavigateTo("Sales & Marketing", "Contacts");
        }

        [Given(@"a user creates new contact")]
        public void GivenAUserCreatesNewContact()
        {
            ContactsList contactsList = new ContactsList(driver);
            contactsList.createContact.Click();

            ContactsCreate contactsCreate = new ContactsCreate(driver);
            contactsCreate.FillCustomerInfo(contactFirstName, contactLastName, role);
            contactsCreate.saveButton.Click();
        }

        [When(@"a user opens latest contact")]
        public void WhenAUserOpensLatestContact()
        {
            ContactsDetails contactsDetails = new ContactsDetails(driver);
            contactsDetails.contactsShortcut.Click();

            ContactsList contactsList = new ContactsList(driver);
            contactsList.SearchCustomer(contactFirstName, contactLastName);
        }

        [Then(@"data from form matches new contact data")]
        public void ThenDataFromFormMatchesNewContactData()
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

        [Given(@"a user goes to Reports&Settings->Reports")]
        public void GivenAUserGoesToReportsSettings_Reports()
        {
            new Dashboard(driver).NavigateTo("Reports & Settings", "Reports");
        }

        [Given(@"a user finds Project Profitability report")]
        public void GivenAUserFindsProjectProfitabilityReport()
        {
            ReportsList reportsList = new ReportsList(driver);
            reportsList.SearchReport(reportName);
        }

        [When(@"a user runs Project Profitability report")]
        public void WhenAUserRunsProjectProfitabilityReport()
        {
            new ReportDetails(driver, reportName).runReportButton.Click();
        }

        [Then(@"that results are returned")]
        public void ThenThatResultsAreReturned()
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(BasePage.ListLocator));
            Assert.That(new ReportDetails(driver, "Project Profitability").CountRowsOnReport(), Is.GreaterThan(0));
        }

        #endregion Run report

        #region Remove events from activity log

        private int beforeDeleting;
        private int afterDeleting;

        [Given(@"a user goes to Reports&Settings->ActivityLog")]
        public void GivenAUserGoesToReportsSettings_ActivityLog()
        {
            new Dashboard(driver).NavigateTo("Reports & Settings", "Activity Log");
        }

        [Given(@"a user selects first (.*) items in the table")]
        public void GivenAUserSelectsFirstItemsInTheTable(int number)
        {
            ActivityList activityList = new ActivityList(driver);

            beforeDeleting = activityList.GetNumberOfLogs();

            activityList.SelectLogs(number);
        }

        [When(@"a user deletes selected items in the table")]
        public void WhenAUserDeletesSelectedItemsInTheTable()
        {
            ActivityList activityList = new ActivityList(driver);

            activityList.DeleteLogs();
        }

        [Then(@"those items are deleted")]
        public void ThenThoseItemsAreDeleted()
        {
            ActivityList activityList = new ActivityList(driver);

            afterDeleting = activityList.GetNumberOfLogs();

            Assert.That(afterDeleting, Is.EqualTo(beforeDeleting - 3));
        }

        #endregion Remove events from activity log
    }
}
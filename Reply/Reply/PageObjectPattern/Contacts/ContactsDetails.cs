using NUnit.Framework;
using OpenQA.Selenium;
using Reply.Tools;

namespace Reply.PageObjectPattern
{
    public class ContactsDetails : BasePage
    {
        public ContactsDetails(IWebDriver driver) : base(driver, "module=Contacts&action=DetailView")
        {
        }

        public IWebElement contactsShortcut => Find(By.XPath("//span[text()='Contacts']/ancestor::a"));

        private IWebElement detailForm => Find(By.Id("DetailForm"));

        public void CheckIfCustomerHasExpectedData(string firstName, string lastName, string role, string category)
        {
            Assert.Multiple(() =>
            {
                Assert.That(detailForm.Text, Does.Contain(firstName));
                Assert.That(detailForm.Text, Does.Contain(lastName));
                Assert.That(detailForm.Text, Does.Contain(role));
                Assert.That(detailForm.Text, Does.Contain(category));
            });
        }
    }
}
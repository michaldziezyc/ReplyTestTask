using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Reply.Tools;

namespace Reply.PageObjectPattern
{
    public class ContactsList : BasePage
    {
        public ContactsList(IWebDriver driver) : base(driver, "module=Contacts&action=index")
        {
        }

        public IWebElement createContact => Find(By.XPath("//button[@name='SubPanel_create']"));

        private IWebElement filterField => Find(By.Id("filter_text"));
        private IList<IWebElement> listOfCustomerNames => FindMultiple(ListLocator).ToList();

        public void SearchCustomer(string firstName, string lastName)
        {
            filterField.SendKeys($"{firstName} {lastName}");
            filterField.SendKeys(Keys.Enter);
            Wait(driver).Until(e => listOfCustomerNames.Count != 20);

            Assert.That(listOfCustomerNames.Count, Is.EqualTo(1));
            listOfCustomerNames.First().Click();
        }
    }
}
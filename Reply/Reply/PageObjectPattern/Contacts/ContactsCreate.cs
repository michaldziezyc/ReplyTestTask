using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using Reply.Tools;
using SeleniumExtras.WaitHelpers;

namespace Reply.PageObjectPattern
{
    public class ContactsCreate : BasePage
    {
        public ContactsCreate(IWebDriver driver) : base(driver, "module=Contacts&action=EditView")
        {
        }

        private IWebElement firstNameField => Find(By.Id("DetailFormfirst_name-input"));
        private IWebElement lastNameField => Find(By.Id("DetailFormlast_name-input"));

        private IWebElement categoryDropDownList => Find(By.Id("DetailFormcategories-input"));

        private IWebElement roleDropDownList => Find(By.Id("DetailFormbusiness_role-input"));

        private By roleSearchFieldLocator => By.XPath("//div[@id='DetailFormcategories-input-search-text']//input");
        private IWebElement roleSearchField => Find(roleSearchFieldLocator);

        private By availableRolesLocator = By.XPath("//div[@class='menu-option single']");
        private IList<IWebElement> availableRoles => FindMultiple(availableRolesLocator).ToList();

        public IWebElement saveButton => Find(By.Id("DetailForm_save2"));

        public void FillCustomerInfo(string firstName, string lastName, string role)
        {
            firstNameField.SendKeys(firstName);
            lastNameField.SendKeys(lastName);
            ChooseCustomerRole(role);
            ChooseCategories();
        }

        public void ChooseCustomerRole(string role)
        {
            roleDropDownList.Click();
            Thread.Sleep(1000);
            availableRoles.First(e => e.Text == role).Click();
        }

        public void ChooseCategories()
        {
            List<string> categories = new List<string>(new string[] { "Customers", "Suppliers" });
            foreach (string category in categories)
            {
                categoryDropDownList.Click();

                Wait(driver).Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(roleSearchFieldLocator));

                roleSearchField.SendKeys(category);

                Find(By.XPath($"//div[contains(@class, 'option-cell input-label') and text() = '{category}']")).Click();

                Thread.Sleep(500);
            }
        }
    }
}
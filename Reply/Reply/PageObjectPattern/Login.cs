using OpenQA.Selenium;
using Reply.Tools;

namespace Reply.PageObjectPattern
{
    public class Login : BasePage
    {
        public Login(IWebDriver driver) : base(driver, "demo.1crmcloud.com/login.php")
        {
        }

        private IWebElement userNameField => Find(By.Id("login_user"));
        private IWebElement passwordField => Find(By.Id("login_pass"));
        private IWebElement button => Find(By.Id("login_button"));

        public void LogIn(string username, string password)
        {
            userNameField.SendKeys(username);
            passwordField.SendKeys(password);
            button.Click();
        }
    }
}
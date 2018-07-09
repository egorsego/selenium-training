using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace User
{
    [TestClass]
    public class UserTest
    {
        private IWebDriver _driver;
        private const string Url = "http://localhost/litecart/en/";

        [TestInitialize]
        public void Setup()
        {
            _driver = Init("chrome");
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(4);
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(Url);
        }

        [TestCleanup]
        public void Teardown()
        {
            _driver.Quit();
        }

        [TestMethod]
        public void SignUpTest()
        {
            String password = GeneratePassword(8);
            String email = GenerateEmail(6);

            _driver.FindElement(By.CssSelector("td > a")).Click();
            _driver.FindElement(By.CssSelector("input[name=firstname]")).SendKeys("John");
            _driver.FindElement(By.CssSelector("input[name=lastname]")).SendKeys("Johnson");
            _driver.FindElement(By.CssSelector("input[name=address1]")).SendKeys("Broadway st.");
            _driver.FindElement(By.CssSelector("input[name=postcode]")).SendKeys("90210");
            _driver.FindElement(By.CssSelector("input[name=city]")).SendKeys("Los Angeles");

            SelectElement countryDropDownElement = new SelectElement(_driver.FindElement(By.CssSelector("select[name=country_code]")));
            countryDropDownElement.SelectByText("United States");

            var numberOfZones = _driver.FindElements(By.CssSelector("select[name=zone_code] > option")).Count;
            SelectElement zoneDropDownElement = new SelectElement(_driver.FindElement(By.CssSelector("select[name=zone_code]")));
            zoneDropDownElement.SelectByIndex(new Random().Next(1, numberOfZones));

            _driver.FindElement(By.CssSelector("input[name=email]")).SendKeys(email);
            _driver.FindElement(By.CssSelector("input[name=phone]")).SendKeys(Keys.Home + "+19991112233");

            _driver.FindElement(By.CssSelector("input[name=password]")).SendKeys(password);
            _driver.FindElement(By.CssSelector("input[name=confirmed_password]")).SendKeys(password);

            _driver.FindElement(By.CssSelector("button[name=create_account]")).Click();

            _driver.FindElement(By.XPath("//div[@id='box-account']//a[contains(text(), 'Logout')]")).Click();

            _driver.FindElement(By.CssSelector("input[name=email]")).SendKeys(email);
            _driver.FindElement(By.CssSelector("input[name=password]")).SendKeys(password);
            _driver.FindElement(By.CssSelector("button[name=login]")).Click();

            _driver.FindElement(By.XPath("//div[@id='box-account']//a[contains(text(), 'Logout')]")).Click();
            
            Assert.IsTrue(_driver.FindElement(By.XPath("//div[@id='notices']//div[contains(text(), ' You are now logged out.')]"))
                .Displayed);
        }

        private string GeneratePassword(int passwordLenght)
        {
            char[] symbols = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()./',".ToCharArray();
            char[] password = new char[passwordLenght];
            Random random = new Random();
            for (int i = 0; i < passwordLenght; i++)
            {
                password[i] = symbols[random.Next(0, symbols.Length - 1)];
            }

            return new string(password);
        }

        private string GenerateEmail(int emailLenght)
        {
            char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower().ToCharArray();
            char[] email = new char[emailLenght];
            Random random = new Random();
            for (int i = 0; i < emailLenght; i++)
            {
                email[i] = alpha[random.Next(0, alpha.Length - 1)];
            }

            return new string(email) + "@email.com";
        }

        private IWebDriver Init(string browser)
        {
            switch (browser.Trim().ToLower())
            {
                case "chrome":
                    return new ChromeDriver();
                case "firefox":
                    return new FirefoxDriver();
                case "edge":
                    return new EdgeDriver();
                default:
                    throw new Exception("Unknown type of browser");
            }
        }
    }
}
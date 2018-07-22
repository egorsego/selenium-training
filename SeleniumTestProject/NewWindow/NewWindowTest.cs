using System;
using System.Collections.ObjectModel;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace NewWindow
{
    [TestClass]
    public class NewWindowTest
    {
        private IWebDriver _driver;
        private const string UrlCountries = "http://localhost/litecart/admin/?app=countries&doc=countries";

        [TestInitialize]
        public void Setup()
        {
            _driver = Init("chrome");
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(UrlCountries);
            LoginAsAdmin();
        }

        [TestCleanup]
        public void Teardown()
        {
            _driver.Quit();
        }

        [TestMethod]
        public void IsNewWindowOpensTest()
        {
            _driver.FindElement(By.XPath("//a[contains(text(),'Add New Country')]")).Click();
            var allNewWindowIcons = _driver.FindElements(By.XPath("//i[@class='fa fa-external-link']"));
            var mainWindow = _driver.CurrentWindowHandle;
            foreach (var newWindowIcon in allNewWindowIcons)
            {
                newWindowIcon.Click();
                
                var newWindow = GetNewWindowHandle(mainWindow);
                _driver.SwitchTo().Window(newWindow);
                _driver.Close();
                _driver.SwitchTo().Window(mainWindow);
            }
            Assert.AreEqual(1, _driver.WindowHandles.Count);
        }

        private void LoginAsAdmin()
        {
            _driver.FindElement(By.CssSelector("#box-login [name=username]")).SendKeys("admin");
            _driver.FindElement(By.CssSelector("#box-login [name=password]")).SendKeys("admin");
            _driver.FindElement(By.CssSelector("#box-login [name=login]")).Click();
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

        private string GetNewWindowHandle(string currentWindow)
        {
            var allWindowHandles = WaitUntilNewWindowOpens();
            string newWindowHandle = string.Empty;

            foreach (var window in allWindowHandles)
            {
                if (!window.Equals(currentWindow))
                {
                    newWindowHandle = window;
                }
            }

            return newWindowHandle;
        }

        private ReadOnlyCollection<string> WaitUntilNewWindowOpens()
        {
            var allWindowHandles = _driver.WindowHandles;
            while (allWindowHandles.Count < 2)
            {
                Thread.Sleep(200);
                allWindowHandles = _driver.WindowHandles;
            }

            return allWindowHandles;
        }
    }
}

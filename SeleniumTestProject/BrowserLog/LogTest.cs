using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace BrowserLog
{
    [TestClass]
    public class LogTest
    {
        private IWebDriver _driver;
        private const string Url = "http://localhost/litecart/admin/?app=catalog&doc=catalog&category_id=1";
        private bool isLogEmpty = true;

        [TestInitialize]
        public void Setup()
        {
            _driver = Init("chrome");
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(Url);
            LoginAsAdmin();
        }

        [TestCleanup]
        public void Teardown()
        {
            _driver.Quit();
        }

        [TestMethod]
        public void IsBrowserLogEmptyTest()
        {
            String allRows = "//tr[@class='row']";
            var numberOfRows = _driver.FindElements(By.XPath(allRows)).Count;
            for (int i = 1; i <= numberOfRows; i++)
            {
                try
                {
                    var currentRow = _driver.FindElement(By.XPath(allRows + "[" + i + "]"));
                    currentRow.FindElement(By.XPath("./td[3]/a[not(contains(text(),'Subcategory'))]")).Click();

                    if (_driver.Manage().Logs.GetLog("browser").Count > 0)
                    {
                        isLogEmpty = false;
                        break;
                    }
                    _driver.Navigate().Back();
                }
                catch (NoSuchElementException)
                {
                    continue;
                }
            }

            Assert.IsTrue(isLogEmpty);
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
    }
}

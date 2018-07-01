using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LitecartNavigation
{
    [TestClass]
    public class LeftNavigationPane
    {
        private IWebDriver _driver;
        private const string Url = "http://localhost/litecart/admin";

        [TestInitialize]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(Url);
        }

        [TestCleanup]
        public void Teardown()
        {
            _driver.Quit();
        }

        [TestMethod]
        public void PaneItemsNavigationTest()
        {
            LoginAsAdmin();

            var numberOfItems = _driver.FindElements(By.XPath("//ul[@id='box-apps-menu']/li[@id='app-']")).Count;
            for (int i = 1; i <= numberOfItems; i++)
            {
                String xPathLocator = "//ul[@id='box-apps-menu']/li[@id='app-']" + "[" + i + "]";
                _driver.FindElement(By.XPath(xPathLocator)).Click();
                var numberOfSubItems = _driver.FindElement(By.XPath(xPathLocator)).FindElements(By.XPath(".//ul[@class='docs'] / li")).Count;
                if (numberOfSubItems != 0)
                {
                    for (int j = 2; j <= numberOfSubItems; j++)
                    {
                        String xPathSubLocator = ".//ul[@class = 'docs'] / li" + "[" + j + "]";
                        _driver.FindElement(By.XPath(xPathSubLocator)).Click();
                        Assert.AreNotEqual(0, _driver.FindElements(By.XPath("//h1")).Count);
                    }
                }
                Assert.AreNotEqual(0, _driver.FindElements(By.XPath("//h1")).Count);
            }
        }

        void LoginAsAdmin()
        {
            _driver.FindElement(By.CssSelector("#box-login [name=username]")).SendKeys("admin");
            _driver.FindElement(By.CssSelector("#box-login [name=password]")).SendKeys("admin");
            _driver.FindElement(By.CssSelector("#box-login [name=login]")).Click();
        }
    }
}

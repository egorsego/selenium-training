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
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
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
            _driver.FindElement(By.CssSelector("#box-login [name=username]")).SendKeys("admin");
            _driver.FindElement(By.CssSelector("#box-login [name=password]")).SendKeys("admin");
            _driver.FindElement(By.XPath("//div[2]/button")).Click();

            var navigationItems = _driver.FindElements(By.CssSelector("#box-apps-menu #app-"));
            foreach (var item in navigationItems)
            {
                item.Click();
            }
        }
    }
}

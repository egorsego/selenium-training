using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace LitecartLogin
{
    [TestClass]
    public class AdminLoginTest
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
        public void IsLoginSuccessfulTest()
        {
            _driver.FindElement(By.XPath("//tbody/tr[1]/td[2]/span/input")).SendKeys("admin");
            _driver.FindElement(By.XPath("//tbody/tr[2]/td[2]/span/input")).SendKeys("admin");
            _driver.FindElement(By.XPath("//div[2]/button")).Click();
            var platform = _driver.FindElement(By.Id("platform")).Text;
            Assert.AreEqual("LiteCart® 1.3.7", platform);
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace LitecartLogin
{
    [TestClass]
    public class AdminLoginTest
    {
        private IWebDriver _driver;
        private const string Url = "https://www.software-testing.ru";

        [TestInitialize]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(Url);
        }

        [TestCleanup]
        public void Teardown()
        {
            _driver.Quit();
        }

        [TestMethod]
        public void PageTitleTest()
        {
            var pageTitle = _driver.Title;
            Assert.AreEqual("Software-Testing.Ru", pageTitle);
        }
    }
}

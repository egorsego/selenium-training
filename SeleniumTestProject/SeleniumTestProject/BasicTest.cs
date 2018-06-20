using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumTestProject
{
    [TestClass]
    public class BasicTest
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
        public void IsPageTitleCorrectTest()
        {
            var pageTitle = _driver.Title;
            Assert.AreEqual("Software-Testing.Ru", pageTitle);
        }
    }
}

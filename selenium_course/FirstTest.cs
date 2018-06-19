using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace selenium_course
{
    [TestClass]
    public class FirstTest
    {
        private IWebDriver _driver;
        private const string Url = "https://www.software-testing.ru";

        [TestInitialize]
        public void Setup()
        {
            _driver = GetBrowserDriver();
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(Url);
        }

        [TestCleanup]
        public void Teardown()
        {
            _driver.Quit();
        }

        [TestMethod]
        public void BasicTest()
        {
            var pageTitle = _driver.Title;
            Assert.AreEqual("Software-Testing.Ru", pageTitle);
        }

        private IWebDriver GetBrowserDriver()
        {
            var outputDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return new ChromeDriver(outputDirectory);
        }
    }
}

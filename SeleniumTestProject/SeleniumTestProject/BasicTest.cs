using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumTestProject
{
    [TestClass]
    public class BasicTest
    {
        private IWebDriver _driver;
        private ChromeOptions _options;
        private const string Url = "https://google.com";

        [TestInitialize]
        public void Setup()
        {
            _options = new ChromeOptions();
            _options.AddArgument("--headless");
            _driver = new ChromeDriver(_options);
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
            Assert.AreEqual("Google", pageTitle);
        }
    }
}

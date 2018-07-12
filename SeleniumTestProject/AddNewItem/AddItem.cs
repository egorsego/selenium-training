using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace AddNewItem
{
    [TestClass]
    public class AddItem
    {
        private IWebDriver _driver;
        private const string Url = "http://localhost/litecart/admin/";
        private String customDuckName = "Sir Rubber Duck";

        [TestInitialize]
        public void Setup()
        {
            _driver = Init("chrome");
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
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
        public void AddNewItemTest()
        {
            if(IsCustomDuckPresent())
                DeleteCustomDuck();

            _driver.FindElement(By.XPath("//a[@class='button'][contains(text(), 'Add New Product')]")).Click();
            _driver.FindElement(By.XPath("//label[contains(text(),'Enabled')]/input")).Click();
            _driver.FindElement(By.XPath("//input[@name='name[en]']")).SendKeys(customDuckName);
            _driver.FindElement(By.XPath("//input[@name='code']")).SendKeys("12345");
            _driver.FindElement(By.XPath("//input[@name='categories[]'][@value='0']")).Click();
            _driver.FindElement(By.XPath("//input[@name='categories[]'][@value='1']")).Click();
            _driver.FindElement(By.XPath("//input[@name='quantity']")).SendKeys(Keys.Delete + "100");

            var path = Path.GetTempPath() + "brit_duck.jpg";
            Resource.brit_duck.Save(path);
            _driver.FindElement(By.XPath("//input[@name='new_images[]']")).SendKeys(path);

            IJavaScriptExecutor js = (IJavaScriptExecutor) _driver;
            js.ExecuteScript("document.querySelector('input[name=date_valid_from]').value='2018-01-01'");
            js.ExecuteScript("document.querySelector('input[name=date_valid_to]').value='2019-12-31'");

            _driver.FindElement(By.XPath("//ul[@class='index']/li/a[contains(text(),'Information')]")).Click();

            SelectElement dropDownElement = new SelectElement(_driver.FindElement(By.XPath("//select[@name='manufacturer_id']")));
            dropDownElement.SelectByText("ACME Corp.");
            _driver.FindElement(By.XPath("//input[@name='keywords']")).SendKeys("Duck, Flag, Great Britain");
            _driver.FindElement(By.XPath("//input[@name='short_description[en]']")).SendKeys("Kwak-Kwak-Kwak");
            _driver.FindElement(By.XPath("//div[@class='trumbowyg-editor']")).SendKeys("Kwak-Kwak-Kwak\nKwak-Kwak-Kwak\nKwak-Kwak-Kwak");
            _driver.FindElement(By.XPath("//input[@name='head_title[en]']")).SendKeys("The Duck of Cambridge");
            _driver.FindElement(By.XPath("//input[@name='meta_description[en]']")).SendKeys("Meta Duck");

            _driver.FindElement(By.XPath("//ul[@class='index']/li/a[contains(text(),'Prices')]")).Click();
            _driver.FindElement(By.XPath("//input[@name='purchase_price']")).SendKeys(Keys.Delete + "5");
            dropDownElement = new SelectElement(_driver.FindElement(By.XPath("//select[@name='purchase_price_currency_code']")));
            dropDownElement.SelectByText("US Dollars");
            _driver.FindElement(By.XPath("//input[@name='prices[USD]']")).SendKeys("10");
            _driver.FindElement(By.XPath("//input[@name='prices[EUR]']")).SendKeys("8");
            _driver.FindElement(By.XPath("//button[@name='save']")).Click();

            Assert.IsTrue(IsCustomDuckPresent());
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

        private bool IsCustomDuckPresent()
        {
            _driver.FindElement(By.XPath("//ul[@id='box-apps-menu']/li//span[contains(text(), 'Catalog')]/../../a")).Click();
            _driver.FindElement(By.XPath("//a[contains(text(), 'Rubber Ducks')]")).Click();
            String customDuckXPath = "//a[contains(text(), " + "'" + customDuckName + "')]";
            return _driver.FindElements(By.XPath(customDuckXPath)).Count > 0;
        }

        private void DeleteCustomDuck()
        {
            String customDuckXPath = "//a[contains(text(), " + "'" + customDuckName + "')]";
            IWebElement customDuck = _driver.FindElement(By.XPath(customDuckXPath));
            customDuck.FindElement(By.XPath("./../../td/input[@type='checkbox']")).Click();
            _driver.FindElement(By.XPath("//button[@name='delete']")).Click();
            IAlert alert = _driver.SwitchTo().Alert();
            alert.Accept();
        }
    }
}

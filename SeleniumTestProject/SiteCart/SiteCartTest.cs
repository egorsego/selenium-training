using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace SiteCart
{
    [TestClass]
    public class SiteCartTest
    {
        private IWebDriver _driver;
        private const string Url = "http://localhost/litecart/en/";

        [TestInitialize]
        public void Setup()
        {
            _driver = Init("chrome");
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(Url);
        }

        [TestCleanup]
        public void Teardown()
        {
            _driver.Quit();
        }

        [TestMethod]
        public void ItemsAddedAndRemovedTest()
        {
            while (GetNumberOfItemsInCart() < 3)
            {
                ShowMainPage();
                GoToFirstItemInfoPage();
                AddItemAndWaitForCounterToUpdate(GetNumberOfItemsInCart());
            }

            GoToSiteCart();
            while (!IsCartEmpty())
            {
                RemoveItemAndWaitForTableToUpdate(GetNumberOfRowsInSiteCartTable());
            }

            ShowMainPage();
            Assert.AreEqual(0, GetNumberOfItemsInCart());
        }

        private IWebDriver Init(string browser)
        {
            switch (browser.Trim().ToLower())
            {
                case "chrome":
                    return new ChromeDriver();
                case "firefox":
                    return new FirefoxDriver();
                default:
                    throw new Exception("Unknown type of browser");
            }
        }

        private void GoToFirstItemInfoPage()
        {
            _driver.FindElement(By.XPath("//div[@id='box-most-popular']//li[1]")).Click();
        }

        private void AddItemToSiteCart()
        {
            string sizeSelectionLocator = "//select[@name='options[Size]']";
            if (_driver.FindElements(By.XPath(sizeSelectionLocator)).Count > 0)
            {
                SelectElement selectedElement = new SelectElement(_driver.FindElement(By.XPath(sizeSelectionLocator)));
                selectedElement.SelectByIndex(1);
            }
            _driver.FindElement(By.XPath("//button[contains(.,'Add To Cart')]")).Click();
        }

        private int GetNumberOfItemsInCart()
        {
            return int.Parse(_driver.FindElement(By.XPath("//span[@class='quantity']")).GetAttribute("textContent"));
        }

        private void AddItemAndWaitForCounterToUpdate(int numberOfItemsBeforeAdding)
        {
            AddItemToSiteCart();

            while (GetNumberOfItemsInCart() == numberOfItemsBeforeAdding)
            {
                Thread.Sleep(200);
            }
        }

        private void ShowMainPage()
        {
            _driver.Navigate().GoToUrl(Url);
        }

        private void GoToSiteCart()
        {
            _driver.FindElement(By.XPath("//a[contains(.,'Checkout')]")).Click();
        }

        private void RemoveItemFromCart()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(3));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[contains(.,'Remove')]"))).Click();
        }

        private int GetNumberOfRowsInSiteCartTable()
        {
            return _driver.FindElements(By.XPath("//td[@class='item']")).Count;
        }

        private void RemoveItemAndWaitForTableToUpdate(int numberOfRowsInTableBeforeRemoving)
        {
            if (IsCartEmpty())
                return;

            RemoveItemFromCart();

            while (GetNumberOfRowsInSiteCartTable() == numberOfRowsInTableBeforeRemoving)
            {
                Thread.Sleep(200);
            }
        }

        private bool IsCartEmpty()
        {
            return _driver.FindElements(By.XPath("//p/em[contains(.,'There are no items in your cart.')]")).Count > 0;
        }
    }
}

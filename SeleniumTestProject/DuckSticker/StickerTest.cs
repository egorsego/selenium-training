using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace DuckSticker
{
    [TestClass]
    public class StickerTest
    {
        private IWebDriver _driver;
        private const string Url = "http://localhost/litecart/en/";

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
        public void EachDuckHasOneStickerTest()
        {
            var mostPopularItemsCollection = "//div[@id='box-most-popular']//li";
            var campaignItemsCollection = "//div[@id='box-campaigns']//li";
            var latestItemsCollection = "//div[@id='box-latest-products']//li";

            bool eachItemHasOneSticker = EachItemHasOneStickerIn(mostPopularItemsCollection) &&
                                         EachItemHasOneStickerIn(campaignItemsCollection) &&
                                         EachItemHasOneStickerIn(latestItemsCollection);
            Assert.IsTrue(eachItemHasOneSticker);
        }

        bool EachItemHasOneStickerIn(String xpathCollectionLocator)
        {
            bool flag = true;
            int numberOfElementsInCollection = _driver.FindElements(By.XPath(xpathCollectionLocator)).Count;
            for (int i = 1; i <= numberOfElementsInCollection; i++)
            {
                String elementInCollectionLocator = xpathCollectionLocator + "[" + i + "]";
                var numberOfStickers = _driver.FindElement(By.XPath(elementInCollectionLocator))
                    .FindElements(By.XPath(".//*[contains(@class, 'sticker')]")).Count;
                if (numberOfStickers != 1)
                    flag = false;
            }
            return flag;
        }
    }
}

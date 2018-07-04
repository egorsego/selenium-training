using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace CampaignDuck
{
    [TestClass]
    public class PriceTest
    {
        private IWebDriver _driver;
        private const string Url = "http://localhost/litecart/en/";

        private String itemNameMainPageLocator = "//div[@id='box-campaigns']//li//div[@class='name']";
        private String itemRegularPriceMainPageLocator = "//div[@id='box-campaigns']//li[1]//div[@class='price-wrapper']/s[@class='regular-price']";
        private String itemCampaignPriceMainPageLocator = "//div[@id='box-campaigns']//li[1]//div[@class='price-wrapper']/strong[@class='campaign-price']";

        private String itemNameCampaignPageLocator = "//h1[@class='title']";
        private String itemRegularPriceCampaignPageLocator = "//div[@id='box-product']//s[@class='regular-price']";
        private String itemCampaignPriceCampaignPageLocator = "//div[@id='box-product']//strong[@class='campaign-price']";

        private String campaignPageLink = "//div[@id='box-campaigns']//a[@class='link']";

        [TestInitialize]
        public void Setup()
        {
            _driver = Init("edge");
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
        public void ItemNameMatchTest()
        {
            var itemNameMainPageElement = _driver.FindElement(By.XPath(itemNameMainPageLocator));
            String itemNameMainPage = GetElementTetxContent(itemNameMainPageElement);

            _driver.FindElement(By.XPath(campaignPageLink)).Click();

            var itemNameCampaignPageElement = _driver.FindElement(By.XPath(itemNameCampaignPageLocator));
            String itemNameCampaignPage = GetElementTetxContent(itemNameCampaignPageElement);

            Assert.AreEqual(itemNameMainPage, itemNameCampaignPage);
        }

        [TestMethod]
        public void RegularPriceMatchTest()
        {
            var regularPriceMainPageElement = _driver.FindElement(By.XPath(itemRegularPriceMainPageLocator));
            int regularPriceMainPage = ParsePrice(GetElementTetxContent(regularPriceMainPageElement));

            _driver.FindElement(By.XPath(campaignPageLink)).Click();

            var regularPriceCampaignPageElement = _driver.FindElement(By.XPath(itemRegularPriceCampaignPageLocator));
            int regularPriceCampaignPage = ParsePrice(GetElementTetxContent(regularPriceCampaignPageElement));

            Assert.AreEqual(regularPriceMainPage, regularPriceCampaignPage);
        }

        [TestMethod]
        public void CampaignPriceMatchTest()
        {
            var campaignPriceMainPageElement = _driver.FindElement(By.XPath(itemCampaignPriceMainPageLocator));
            int campaignPriceMainPage = ParsePrice(GetElementTetxContent(campaignPriceMainPageElement));

            _driver.FindElement(By.XPath(campaignPageLink)).Click();

            var campaignPriceCampaingPageElement = _driver.FindElement(By.XPath(itemCampaignPriceCampaignPageLocator));
            int campaignPriceCampaignPage = ParsePrice(GetElementTetxContent(campaignPriceCampaingPageElement));

            Assert.AreEqual(campaignPriceMainPage, campaignPriceCampaignPage);
        }

        [TestMethod]
        public void RegularPriceIsGreyOnMainPageTest()
        {
            var regularPriceMainPageElement = _driver.FindElement(By.XPath(itemRegularPriceMainPageLocator));
            var regularPriceMainPageColorRaw = regularPriceMainPageElement.GetCssValue("color");
            Color regularPriceMainPageColor = new Color(ParseColor(regularPriceMainPageColorRaw));

            Assert.IsTrue(regularPriceMainPageColor.IsGrey());
        }

        [TestMethod]
        public void RegularPriceIsGreyOnCampaignPageTest()
        {
            _driver.FindElement(By.XPath(campaignPageLink)).Click();

            var regularPriceCampaignPageElement = _driver.FindElement(By.XPath(itemRegularPriceCampaignPageLocator));
            var regularPriceCampaignPageColorRaw = regularPriceCampaignPageElement.GetCssValue("color");
            Color regularPriceCampaignPageColor = new Color(ParseColor(regularPriceCampaignPageColorRaw));

            Assert.IsTrue(regularPriceCampaignPageColor.IsGrey());
        }

        [TestMethod]
        public void RegularPriceIsLineThroughOnMainPageTest()
        {
            var regularPriceMainPageElement = _driver.FindElement(By.XPath(itemRegularPriceMainPageLocator));
            var regularPriceMainPageTextDecoration = regularPriceMainPageElement.GetCssValue("text-decoration").Contains("line-through");

            Assert.IsTrue(regularPriceMainPageTextDecoration);
        }

        [TestMethod]
        public void RegularPriceIsLineThroughOnCampaignPageTest()
        {
            _driver.FindElement(By.XPath(campaignPageLink)).Click();

            var regularPriceCampaignPageElement = _driver.FindElement(By.XPath(itemRegularPriceCampaignPageLocator));
            var regularPriceCampaignPageTextDecoration = regularPriceCampaignPageElement.GetCssValue("text-decoration").Contains("line-through");

            Assert.IsTrue(regularPriceCampaignPageTextDecoration);
        }

        [TestMethod]
        public void CampaignPriceIsRedOnMainPageTest()
        {
            var campaignPriceMainPageElement = _driver.FindElement(By.XPath(itemCampaignPriceMainPageLocator));
            var campaignPriceMainPageColorRaw = campaignPriceMainPageElement.GetCssValue("color");
            Color campaignPriceMainPageColor = new Color(ParseColor(campaignPriceMainPageColorRaw));

            Assert.IsTrue(campaignPriceMainPageColor.IsRed());
        }

        [TestMethod]
        public void CampaignPriceIsRedOnCampaignPageTest()
        {
            _driver.FindElement(By.XPath(campaignPageLink)).Click();

            var campaignPriceCampaingPageElement = _driver.FindElement(By.XPath(itemCampaignPriceCampaignPageLocator));
            var campaignPriceCampaignPageColorRaw = campaignPriceCampaingPageElement.GetCssValue("color");
            Color campaignPriceCampaignPageColor = new Color(ParseColor(campaignPriceCampaignPageColorRaw));

            Assert.IsTrue(campaignPriceCampaignPageColor.IsRed());
        }

        [TestMethod]
        public void CampaignPriceIsBoldOnMainPageTest()
        {
            var campaignPriceMainPageElement = _driver.FindElement(By.XPath(itemCampaignPriceMainPageLocator));
            var campaignPriceMainPageFontWeight = int.Parse(campaignPriceMainPageElement.GetCssValue("font-weight"));

            Assert.IsTrue(campaignPriceMainPageFontWeight >= 700);
        }

        [TestMethod]
        public void CampaignPriceIsBoldOnCampaignPageTest()
        {
            _driver.FindElement(By.XPath(campaignPageLink)).Click();

            var campaignPriceCampaingPageElement = _driver.FindElement(By.XPath(itemCampaignPriceCampaignPageLocator));
            var campaignPriceCampaignPageFontWeight = int.Parse(campaignPriceCampaingPageElement.GetCssValue("font-weight"));

            Assert.IsTrue(campaignPriceCampaignPageFontWeight >= 700);
        }

        [TestMethod]
        public void CampaignFontSizeIsBiggerOnMainPageTest()
        {
            var regularPriceMainPageElement = _driver.FindElement(By.XPath(itemRegularPriceMainPageLocator));
            var regularPriceMainPageFontSizeRaw = regularPriceMainPageElement.GetCssValue("font-size");
            double regularPriceMainPageFontSize = ParseFontSize(regularPriceMainPageFontSizeRaw);

            var campaignPriceMainPageElement = _driver.FindElement(By.XPath(itemCampaignPriceMainPageLocator));
            var campaignPriceMainPageFontSizeRaw = campaignPriceMainPageElement.GetCssValue("font-size");
            double campaignPriceMainPageFontSize = ParseFontSize(campaignPriceMainPageFontSizeRaw);

            Assert.IsTrue(campaignPriceMainPageFontSize > regularPriceMainPageFontSize);
        }

        [TestMethod]
        public void CampaignFontSizeIsBiggerOnCampaignPageTest()
        {
            _driver.FindElement(By.XPath(campaignPageLink)).Click();

            var regularPriceCampaignPageElement = _driver.FindElement(By.XPath(itemRegularPriceCampaignPageLocator));
            var regularPriceCampaignPageFontSizeRaw = regularPriceCampaignPageElement.GetCssValue("font-size");
            double regularPriceCampaignPageFontSize = ParseFontSize(regularPriceCampaignPageFontSizeRaw);

            var campaignPriceCampaingPageElement = _driver.FindElement(By.XPath(itemCampaignPriceCampaignPageLocator));
            var campaignPriceCampaignPageFontSizeRaw = campaignPriceCampaingPageElement.GetCssValue("font-size");
            double campaignPriceCampaignPageFontSize = ParseFontSize(campaignPriceCampaignPageFontSizeRaw);

            Assert.IsTrue(campaignPriceCampaignPageFontSize > regularPriceCampaignPageFontSize);
        }

        private int ParsePrice(String priceString)
        {
            Regex regexPattern = new Regex(@"^\$(\d+)$");
            Match match = regexPattern.Match(priceString);
            if (match.Success)
            {
                return int.Parse(match.Groups[1].Value);
            }
            throw new Exception("Unable to parse the price: " + match.Value);
        }

        private double ParseFontSize(String fontSizeString)
        {
            Regex regexPattern = new Regex(@"^(\d+\.?\d*)px$");
            Match match = regexPattern.Match(fontSizeString);
            if (match.Success)
            {
                return double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
            }
            throw new Exception("Unable to parse the font size: " + match.Value);
        }

        private int[] ParseColor(String colorString)
        {
            int[] rgbArray = new int[3];
            Regex regexPattern = new Regex(@"^rgb\((\d+)\,\s(\d+)\,\s(\d+)\)$");

            if (_driver is ChromeDriver)
                regexPattern = new Regex(@"^rgba\((\d+)\,\s(\d+)\,\s(\d+)\,\s(\d+)\)$");

            Match match = regexPattern.Match(colorString);
            if (match.Success)
            {
                rgbArray[0] = int.Parse(match.Groups[1].Value);
                rgbArray[1] = int.Parse(match.Groups[2].Value);
                rgbArray[2] = int.Parse(match.Groups[3].Value);
                return rgbArray;
            }
            throw new Exception("Unable to parse the color: " + match.Value);
        }

        private String GetElementTetxContent(IWebElement element)
        {
            return element.GetAttribute("textContent");
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
    }
}

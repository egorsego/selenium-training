using OpenQA.Selenium;

namespace PageObjectCart
{
    class MainPage
    {
        private string Url = "http://localhost/litecart/en/";
        private readonly IWebDriver _driver;

        public MainPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void Open()
        {
            _driver.Navigate().GoToUrl(Url);
        }

        public void OpenFirstItemPage()
        {
            _driver.FindElement(By.XPath("//div[@id='box-most-popular']//li[1]")).Click();
        }

        public void OpenSiteCart()
        {
            _driver.FindElement(By.XPath("//a[contains(.,'Checkout')]")).Click();
        }

        public int GetNumberOfItemsInCart()
        {
            return int.Parse(_driver.FindElement(By.XPath("//span[@class='quantity']")).GetAttribute("textContent"));
        }
    }
}

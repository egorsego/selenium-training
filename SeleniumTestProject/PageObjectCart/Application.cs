using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace PageObjectCart
{
    class Application
    {
        private readonly IWebDriver _driver;
        public MainPage MainPage;
        public ItemPage ItemPage;
        public CartPage CartPage;

        public Application()
        {
            _driver = Init("Chrome");
            _driver.Manage().Window.Maximize();
            MainPage = new MainPage(_driver);
            ItemPage = new ItemPage(_driver);
            CartPage = new CartPage(_driver);
        }

        public void Quit()
        {
            _driver.Quit();
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
    }
}

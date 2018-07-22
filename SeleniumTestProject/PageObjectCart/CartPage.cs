using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace PageObjectCart
{
    class CartPage
    {
        private readonly IWebDriver _driver;

        public CartPage(IWebDriver driver)
        {
            _driver = driver;
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

        public bool IsCartEmpty()
        {
            return _driver.FindElements(By.XPath("//p/em[contains(.,'There are no items in your cart.')]")).Count > 0;
        }

        public void RemoveItemAndWaitForTableToUpdate()
        {
            if (IsCartEmpty())
                return;

            int numberOfRowsInTableBeforeRemoving = GetNumberOfRowsInSiteCartTable();

            RemoveItemFromCart();

            while (GetNumberOfRowsInSiteCartTable() == numberOfRowsInTableBeforeRemoving)
            {
                Thread.Sleep(200);
            }
        }
    }


}

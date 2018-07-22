using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace PageObjectCart
{
    class ItemPage
    {
        private readonly IWebDriver _driver;

        public ItemPage(IWebDriver driver)
        {
            _driver = driver;
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

        public void AddItemAndWaitForCounterToUpdate()
        {
            int numberOfItemsBeforeAdding = GetNumberOfItemsInCart();

            AddItemToSiteCart();

            while (GetNumberOfItemsInCart() == numberOfItemsBeforeAdding)
            {
                Thread.Sleep(200);
            }
        }

        public void OpenSiteCart()
        {
            _driver.FindElement(By.XPath("//a[contains(.,'Checkout')]")).Click();
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PageObjectCart
{
    [TestClass]
    public class SiteCartTest
    {
        private Application app;

        [TestInitialize]
        public void Setup()
        {
            app = new Application();
        }

        [TestCleanup]
        public void Teardown()
        {
            app.Quit();
        }

        [TestMethod]
        public void AddRemoveItemsTest()
        {
            for (int i = 0; i <= 2; i++)
            {
                app.MainPage.Open();
                app.MainPage.OpenFirstItemPage();
                app.ItemPage.AddItemAndWaitForCounterToUpdate();
            }

            app.ItemPage.OpenSiteCart();

            while (!app.CartPage.IsCartEmpty())
            {
                app.CartPage.RemoveItemAndWaitForTableToUpdate();
            }

            app.MainPage.Open();

            Assert.AreEqual(0, app.MainPage.GetNumberOfItemsInCart());
        }
    }
}

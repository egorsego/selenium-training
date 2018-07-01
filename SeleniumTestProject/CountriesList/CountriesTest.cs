using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CountriesList
{
    [TestClass]
    public class CountriesTest
    {
        private IWebDriver _driver;
        private const string Url = "http://localhost/litecart/admin/?app=countries&doc=countries";

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
        public void AreCoutntriesInAlphabeticOrderTest()
        {
            LoginAsAdmin();

            var allCountryRows = "//table/tbody/tr[@class='row']";
            var numberOfCountries = _driver.FindElements(By.XPath(allCountryRows)).Count;

            ArrayList listOfCountries = new ArrayList();
            ArrayList listOfZones = new ArrayList();

            for (int i = 1; i <= numberOfCountries; i++)
            {
                String countryRow = allCountryRows + "[" + i + "]";
                String countryName = countryRow + "/td[5]";
                String countryZones = countryRow + "/td[6]";

                String country = _driver.FindElement(By.XPath(countryName)).GetAttribute("textContent");
                listOfCountries.Add(country);

                var countryZonesNumber = int.Parse(_driver.FindElement(By.XPath(countryZones)).GetAttribute("textContent"));
                if (countryZonesNumber > 0)
                {
                    _driver.FindElement(By.XPath(countryName + "/a")).Click();
                    listOfZones.Clear();
                    var allZoneRows = "//table[@id='table-zones']/tbody/tr[not(@class='header')]";
                    for (int j = 1; j <= countryZonesNumber; j++)
                    {
                        String zoneRow = allZoneRows + "[" + j + "]";
                        String zoneName = zoneRow + "/td[3]";

                        String zone = _driver.FindElement(By.XPath(zoneName)).GetAttribute("textContent");
                        listOfZones.Add(zone);
                    }

                    ArrayList sortedListOfZones = new ArrayList(listOfZones);
                    sortedListOfZones.Sort();

                    Assert.IsTrue(AreListsMatch(listOfZones, sortedListOfZones));

                    _driver.Navigate().Back();
                }
            }
            ArrayList sortedListOfCountries = new ArrayList (listOfCountries);
            sortedListOfCountries.Sort();

            Assert.IsTrue(AreListsMatch(listOfCountries, sortedListOfCountries));
        }

        private void LoginAsAdmin()
        {
            _driver.FindElement(By.CssSelector("#box-login [name=username]")).SendKeys("admin");
            _driver.FindElement(By.CssSelector("#box-login [name=password]")).SendKeys("admin");
            _driver.FindElement(By.CssSelector("#box-login [name=login]")).Click();
        }

        private static bool AreListsMatch(ArrayList unsortedList, ArrayList sortedList)
        {
            for (int i = 0; i < unsortedList.Count; i++)
            {
                if (!unsortedList[i].Equals(sortedList[i]))
                    return false;
            }
            return true;
        }
    }
}

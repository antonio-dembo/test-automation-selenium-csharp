using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Threading;

namespace ShadowDom
{
    [TestClass]
    public class ShadowDOMExampleTest
    {
        private IWebDriver _driver;
        private const string driverPath = @"D:\webdriver\chromedriver_win32";

        [TestInitialize]
        public void Initialization()
        {
            Console.WriteLine("Opening chrome browser");
            ChromeOptions options = new ChromeOptions();

            options.AddArgument("start-maximized");
            _driver = new ChromeDriver(chromeDriverDirectory: driverPath, options);
        }

        [TestMethod]
        public void TestGetText_FromShadowDOMElements()
        {
            Console.WriteLine("Open Chrome downloads");
            _driver.Navigate().GoToUrl("chrome://downloads");

            Console.WriteLine("Validate downloads page header text");
            IWebElement shadowRoot = GetLastSelectorShadowroot("downloads-manager", "downloads-toolbar", "cr-toolbar");            
            var actualHeading = shadowRoot.FindElement(By.XPath("//div[@id='leftContent']/div/"));  //this line breaks the test
            string actualHeadingText = actualHeading.Text;
            Assert.Equals("Downloads", actualHeadingText);
        }

        private IWebElement GetLastSelectorShadowroot( params string[] selectors)
        {
            var js = (IJavaScriptExecutor)_driver;
            var root = js.ExecuteScript("return document");
           
            foreach ( string selector in selectors)
            {
                root = js.ExecuteScript("return arguments[0].querySelector(arguments[1]).shadowRoot", root, selector);
            };

            return (IWebElement) root;
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _driver.Quit();
        }
    }
}

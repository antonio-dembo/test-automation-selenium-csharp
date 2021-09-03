using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace ShadowDom
{
    [TestClass]
    public class ShadowDOMExampleTest
    {
        private IWebDriver _driver;
        //private const string driverPath = @"D:\webdriver\chromedriver_win32";

        [TestInitialize]
        public void Initialization()
        {
            Console.WriteLine("Opening chrome browser");
            ChromeOptions options = new ChromeOptions();

            options.AddArgument("start-maximized");
            _driver = new ChromeDriver(options);
        }

        [TestMethod]
        public void TestGetText_FromShadowDOMElements()
        {
            Console.WriteLine("Open Chrome downloads");
            _driver.Navigate().GoToUrl("chrome://downloads");

            Console.WriteLine("Validate downloads page header text");
            IWebElement shadowRoot = GetLastSelectorShadowroot("downloads-manager", "downloads-toolbar", "toolbar");            
            var actualHeading = shadowRoot.FindElement(By.Id("leftSpacer"));
            
            string actualHeadingText = actualHeading.Text;
            Assert.AreEqual("Downloads", actualHeadingText);
        }

        private IWebElement GetLastSelectorShadowroot( params string[] selectors)
        {
            var js = (IJavaScriptExecutor)_driver;
            var root = js.ExecuteScript("return document");

            string namedItemObjectJS = "return arguments[0].children.namedItem(arguments[1])";
            string querySelectorJS = "return arguments[0].querySelector(arguments[1]).shadowRoot";
            string getElementByIDJS = "return arguments[0].getElementById(arguments[1]).shadowRoot";

            try
            {
                foreach (string selector in selectors)
                {
                    var checkElement = js.ExecuteScript(namedItemObjectJS, root, selector);

                    if (checkElement is null)
                    {
                        root = js.ExecuteScript(querySelectorJS, root, selector);
                        continue;
                    }
                    root = js.ExecuteScript(getElementByIDJS, root, selector);
                };
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                throw;
            }

            return (IWebElement) root;
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _driver.Quit();
        }
    }
}

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
            _driver.Navigate().GoToUrl("chrome://downloads");

            //Get shadow root element
            IWebElement root1 = _driver.FindElement(By.TagName("downloads-manager"));
            IWebElement shadowRoot1 = expandRootElement(root1);

            IWebElement root2 = shadowRoot1.FindElement(By.TagName("downloads-toolbar"));
            IWebElement shadowRoot2 = expandRootElement(root2);

            IWebElement root3 = shadowRoot2.FindElement(By.TagName("cr-toolbar"));
            IWebElement shadowRoot3 = expandRootElement(root3);

            //this line breaks the test
            var actualHeading = shadowRoot3.FindElements(By.XPath("//div[@id='leftContent']/div/"));
            
            string actualHeadingText = actualHeading[1].Text;

            Assert.Equals("Downloads", actualHeadingText);


        }

        private IWebElement expandRootElement(IWebElement element)
        {
            return (IWebElement)((IJavaScriptExecutor)_driver ).ExecuteScript("return arguments[0].shadowRoot", element);
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _driver.Quit();
        }
    }
}

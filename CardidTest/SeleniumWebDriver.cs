using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace CardidTest
{
    [TestClass]
    public class SeleniumWebDriver
    {
        static IWebDriver driverFF;
        static IWebDriver driverGC;

        [AssemblyInitialize]
        public static void Setup(TestContext context)
        {
            driverFF = new FirefoxDriver();
            driverGC = new ChromeDriver();
        }

        [TestMethod]
        public void TestFirefoxDriver()
        {
            string url = "https://cardid.apphb.com/";
            driverFF.Navigate().GoToUrl(url);
            driverFF.Quit();
        }

        [TestMethod]
        public void TestChromeDriver()
        {
            string url = "https://cardid.apphb.com/";
            driverGC.Navigate().GoToUrl(url);
            driverGC.Quit();
        }

    }
}
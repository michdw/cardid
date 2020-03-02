using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace CardidTest
{
    [TestClass]
    public class SeleniumFirefoxDriver
    {
        static IWebDriver driverFF;

        [AssemblyInitialize]
        public static void Setup()
        {
            driverFF = new FirefoxDriver();
        }

        [TestMethod]
        public void RegisterMember()
        {
            string url = "https://cardid.apphb.com/";
            driverFF.Navigate().GoToUrl(url);
            IWebElement regButton = driverFF.FindElement(By.CssSelector("[href='/Home/BeginRegister']"));
            regButton.Click();

            driverFF.Quit();

        }

    }
}

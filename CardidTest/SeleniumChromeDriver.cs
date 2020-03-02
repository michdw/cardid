using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CardidTest
{
    [TestClass]
    public class SeleniumChromeDriver
    {
        static IWebDriver driverGC;

        [AssemblyInitialize]
        public static void Setup()
        {
            driverGC = new ChromeDriver();
        }

        [TestMethod]
        public void RegisterMember()
        {
            string url = "https://cardid.apphb.com/";
            driverGC.Navigate().GoToUrl(url);
            driverGC.FindElement(By.CssSelector("[href='/Home/BeginRegister']"))
                .Click();
            driverGC.FindElement(By.Id("DisplayName"))
                .SendKeys("TestMember");
            driverGC.FindElement(By.Id("Email"))
                .SendKeys("testmember@cardid.co");
            driverGC.FindElement(By.Id("Password"))
                .SendKeys("testpw");
            driverGC.FindElement(By.Id("ConfirmPassword"))
                .SendKeys("testpw");
            driverGC.FindElement(By.XPath("//button[@type='submit']"))
                .Click();
            driverGC.FindElement(By.CssSelector("[href='/Home/Logout']"))
                .Click();
            driverGC.FindElement(By.CssSelector("[href='/Home/Login']"))
                .Click();
            driverGC.FindElement(By.Id("Email"))
                .SendKeys("TestMember");
            driverGC.FindElement(By.Id("Password"))
                .SendKeys("testpw");
            driverGC.FindElement(By.XPath("//button[@type='submit']"))
                .Click();
            driverGC.Quit();
        }
    }
}
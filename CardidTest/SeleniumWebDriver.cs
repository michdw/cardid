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
        IWebElement htmlBody;

        [AssemblyInitialize]
        public static void Setup(TestContext context)
        {
            //driverFF = new FirefoxDriver();
            driverGC = new ChromeDriver();
        }

        [TestMethod]
        public void TestChromeDriver()
        {
            string url = "https://cardid.apphb.com/";
            driverGC.Navigate().GoToUrl(url);

            //register new member
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
            htmlBody = driverGC.FindElement(By.TagName("body"));
            Assert.IsTrue(htmlBody.Text.Contains("Welcome, TestMember!"));

            //logout and log back in
            driverGC.FindElement(By.CssSelector("[href='/Home/Logout']"))
                .Click();
            htmlBody = driverGC.FindElement(By.TagName("body"));
            Assert.IsTrue(htmlBody.Text.Contains("You have been logged out"));

            driverGC.FindElement(By.CssSelector("[href='/Home/Login']"))
                .Click();
            driverGC.FindElement(By.Id("Email"))
                .SendKeys("testmember@cardid.co");
            driverGC.FindElement(By.Id("Password"))
                .SendKeys("testpw");
            driverGC.FindElement(By.XPath("//button[@type='submit']"))
                .Click();
            htmlBody = driverGC.FindElement(By.TagName("body"));
            Assert.IsTrue(htmlBody.Text.Contains("Welcome back, TestMember!"));

            //create deck
            driverGC.FindElement(By.CssSelector("[href='/Deck/CreateDeckInit']"))
                .Click();
            IWebElement deckForm = driverGC.FindElement(By.ClassName("create-deck"));
            deckForm.FindElement(By.TagName("input"))
                .SendKeys("testdeck");
            deckForm.FindElement(By.TagName("button"))
                .Click();

            //add cards
            IWebElement cardForm = driverGC.FindElement(By.ClassName("card-blank")).FindElement(By.TagName("form"));
            cardForm.FindElement(By.Id("NewCardFront"))
                .SendKeys("front 1");
            cardForm.FindElement(By.Id("NewCardBack"))
                .SendKeys("back 1");
            cardForm.FindElement(By.TagName("button"))
                .Click();
            cardForm = driverGC.FindElement(By.ClassName("card-blank")).FindElement(By.TagName("form"));
            cardForm.FindElement(By.Id("NewCardFront"))
                .SendKeys("front 2");
            cardForm.FindElement(By.Id("NewCardBack"))
                .SendKeys("back 2");
            cardForm.FindElement(By.TagName("button"))
                .Click();

            //add new tag to deck
            IWebElement tagForm = driverGC.FindElement(By.CssSelector("[action='/Deck/CreateTagForDeck']"));
            tagForm.FindElement(By.TagName("input"))
                .SendKeys("testtag");
            tagForm.FindElement(By.TagName("button"))
                .Click();

            ////find deck by tag

            //study session

            //delete account
            driverGC.FindElement(By.CssSelector("[href='/Home/Account']"))
                .Click();
            driverGC.FindElement(By.CssSelector("[href='/Home/ChangeInfoInit']"))
                .Click();
            IWebElement userInfo = driverGC.FindElement(By.ClassName("userinfo-module"));
            userInfo.FindElement(By.ClassName("delete-btn"))
                .Click();
            IWebElement deleteForm = driverGC.FindElement(By.ClassName("delete-submit"));
            deleteForm.FindElement(By.ClassName("delete-btn"))
                .Click();
            htmlBody = driverGC.FindElement(By.TagName("body"));
            Assert.IsTrue(htmlBody.Text.Contains("Account has been deleted"));

            //close driver
            driverGC.Quit();

        }

        //[TestMethod]
        //public void TestFirefoxDriver()
        //{
        //    string url = "https://cardid.apphb.com/";
        //    driverFF.Navigate().GoToUrl(url);
        //    //register new member
        //    driverFF.FindElement(By.CssSelector("[href='/Home/BeginRegister']"))
        //        .Click();
        //    driverFF.FindElement(By.Id("DisplayName"))
        //        .SendKeys("TestMember");
        //    driverFF.FindElement(By.Id("Email"))
        //        .SendKeys("testmember@cardid.co");
        //    driverFF.FindElement(By.Id("Password"))
        //        .SendKeys("testpw");
        //    driverFF.FindElement(By.Id("ConfirmPassword"))
        //        .SendKeys("testpw");
        //    driverFF.FindElement(By.XPath("//button[@type='submit']"))
        //        .Click();
        //    //logout and log back in
        //    driverFF.FindElement(By.CssSelector("[href='/Home/Logout']"))
        //        .Click();
        //    driverFF.FindElement(By.CssSelector("[href='/Home/Login']"))
        //        .Click();
        //    driverFF.FindElement(By.Id("Email"))
        //        .SendKeys("testmember@cardid.co");
        //    driverFF.FindElement(By.Id("Password"))
        //        .SendKeys("testpw");
        //    driverFF.FindElement(By.XPath("//button[@type='submit']"))
        //        .Click();
        //    //delete account
        //    driverFF.FindElement(By.CssSelector("[href='/Home/Account']"))
        //        .Click();
        //    driverFF.FindElement(By.CssSelector("[href='/Home/ChangeInfoInit']"))
        //        .Click();
        //    driverFF.FindElement(By.ClassName("delete-init"))
        //        .Click();
        //    driverFF.FindElement(By.ClassName("delete-submit"))
        //        .Click();
        //    //close driver
        //    driverFF.Quit();
        //}

    }
}
﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace Slowlenium
{
    [TestFixture]
    public class SlowTest
    {
        [Test]
        public void IShouldOpenABrowserAndDoSomeStuffThatTakesAWhile()
        {
            IWebDriver driver = WebDriverFactory.GetDriver();
            driver.Navigate().GoToUrl("http://www.google.com");
            Thread.Sleep(5000);
            driver.Quit();
        }
    }
}

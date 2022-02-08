using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;


namespace Webscraper
{
    public class Scraper : IDisposable
    {
        public IWebDriver Driver { get; set; }
        public DriverOptions Options { get; set; }
        public WebDriverWait Wait { get; set; }

        /// <summary>
        /// Handles a Webscraper, which runs headless in release mode<br></br>
        /// Sets a Wait for use in primary flows which timeouts after 10 seconds with a 1 second polling interval. The wait ignores NoSuchElementExceptions.<br></br>
        /// Works on sites without a legitimate SSL Certificate.
        /// </summary>
        public Scraper()
        {
            //Set Options
            Options = new FirefoxOptions()
            {
                AcceptInsecureCertificates = true,
                EnableDevToolsProtocol = true
            };
#if !DEBUG
            ((FirefoxOptions)Options).AddArgument("--headless");
#endif

            //Start driver
            Driver = new FirefoxDriver((FirefoxOptions)Options);

            //Set wait
            SetupDefaultWait();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void SetupDefaultWait()
        {
            Wait = new(Driver, TimeSpan.FromSeconds(25))
            {
                PollingInterval = TimeSpan.FromSeconds(1)
            };
            Wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
        }

        /// <summary>
        /// Test method for verifying WebScraper is functional.
        /// </summary>
        /// <returns>Returns an interesting fact from Wikipedias frontpage</returns>
        public string HelloWorld()
        {
            //Setup
            Driver = new FirefoxDriver();
            WebDriverWait wait = new(Driver, TimeSpan.FromSeconds(10));

            //Go to wikipedia
            Driver.Navigate().GoToUrl(@"https://en.wikipedia.org/wiki/Main_Page");

            //Pick DYK fact
            wait.Until(webDriver => webDriver.FindElement(By.Id("mp-dyk")).Displayed);
            IWebElement dykDiv = Driver.FindElement(By.Id("mp-dyk"));
            IWebElement firstFactdykDiv = dykDiv.FindElement(By.TagName("li"));

            //return string to caller
            string output = "Did you know?\n";
            output += firstFactdykDiv.Text;

            return output;
        }

        /// <summary>
        /// Disposes WebDriver, handled by Using statements.
        /// </summary>
        public void Dispose()
        {
            Driver.Dispose();
        }
    }
}

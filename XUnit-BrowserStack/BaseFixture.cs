using OpenQA.Selenium.Remote;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium;
using BrowserStack;

namespace XUnit_BrowserStack
{
    public class BaseFixture : IDisposable
    {

        private RemoteWebDriver? WebDriver;

        public RemoteWebDriver GetDriver(string platform, string profile)
        {

            // Create Desired Cappabilities for WebDriver
            DriverOptions desiredCapabilities = new OpenQA.Selenium.Chrome.ChromeOptions();

            // Create RemoteWebDriver instance
            WebDriver = new RemoteWebDriver(new Uri($"http://localhost:4444/wd/hub"), desiredCapabilities);

            return WebDriver;
        }

        public void Dispose()
        {
            if (WebDriver is not null)
            {
                WebDriver.Quit();
                WebDriver.Dispose();
            }
        }
    }
}

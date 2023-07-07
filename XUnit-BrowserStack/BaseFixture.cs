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
        private Local? browserStackLocal;


        public RemoteWebDriver GetDriver(string platform, string profile)
        {
            // Get Configuration for correct profile
            string currentDirectory = Directory.GetCurrentDirectory();
            string path = Path.Combine(currentDirectory, "config.json");
            JObject config = JObject.Parse(File.ReadAllText(path));
            if (config is null)
                throw new Exception("Configuration not found!");

            // Get Platform specific capabilities
            JObject capabilities = config.GetValue("environments").Where(x => x is JObject y && x["browserName"].ToString().Equals(platform)).ToList()[0] as JObject;

            // Get Common Capabilities
            JObject commonCapabilities = config.GetValue("capabilities") as JObject;

            // Merge Capabilities
            capabilities.Merge(commonCapabilities);

            JObject bstackOptions = capabilities["bstack:options"] as JObject;

            // Get username and accesskey
            string? username = Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME");
            if (username is null)
                username = config.GetValue("user").ToString();

            string? accessKey = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY");
            if (accessKey is null)
                accessKey = config.GetValue("key").ToString();

            bstackOptions["userName"] = username;
            bstackOptions["accessKey"] = accessKey;
            
            // Start Local if browserstack.local is set to true
            if (profile.Equals("local") && accessKey is not null)
            {
                bstackOptions["local"] = true;
                browserStackLocal = new Local();
                List<KeyValuePair<string, string>> bsLocalArgs = new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("key", accessKey)
                };
                foreach (var localOption in config.GetValue("localOptions") as JObject)
                {
                    if (localOption.Value is not null)
                    {
                        bsLocalArgs.Add(new KeyValuePair<string, string>(localOption.Key, localOption.Value.ToString()));
                    }
                }
                browserStackLocal.start(bsLocalArgs);
            }

            capabilities["bstack:options"] = bstackOptions;

            // Create Desired Cappabilities for WebDriver
            DriverOptions desiredCapabilities = getBrowserOption(capabilities.GetValue("browserName").ToString());
            capabilities.Remove("browserName");
            foreach (var x in capabilities)
            {
                if (x.Key.Equals("bstack:options"))
                    desiredCapabilities.AddAdditionalOption(x.Key, x.Value.ToObject<Dictionary<string, object>>());
                else
                    desiredCapabilities.AddAdditionalOption(x.Key, x.Value);
            }

            // Create RemoteWebDriver instance
            WebDriver = new RemoteWebDriver(new Uri($"https://{config["server"]}/wd/hub"), desiredCapabilities);

            return WebDriver;
        }

        public void SetStatus(bool passed)
        {
            if(WebDriver is not null)
            {
                if (passed)
                    ((IJavaScriptExecutor)WebDriver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"passed\", \"reason\": \"Test Passed!\"}}");
                else
                    ((IJavaScriptExecutor)WebDriver).ExecuteScript("browserstack_executor: {\"action\": \"setSessionStatus\", \"arguments\": {\"status\":\"failed\", \"reason\": \"Test Failed!\"}}");
            }
        }

        static DriverOptions getBrowserOption(String browser)
        {
            switch (browser)
            {
                case "chrome":
                    return new OpenQA.Selenium.Chrome.ChromeOptions();
                case "firefox":
                    return new OpenQA.Selenium.Firefox.FirefoxOptions();
                case "safari":
                    return new OpenQA.Selenium.Safari.SafariOptions();
                case "edge":
                    return new OpenQA.Selenium.Edge.EdgeOptions();
                default:
                    return new OpenQA.Selenium.Chrome.ChromeOptions();
            }
        }

        public void Dispose()
        {
            if (WebDriver is not null)
            {
                WebDriver.Quit();
                WebDriver.Dispose();
            }
            if(browserStackLocal is not null)
            {
                browserStackLocal.stop();
            }
        }
    }
}

using OpenQA.Selenium.Remote;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium.Chrome;
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
            JObject capabilities = config.GetValue("environments").Where(x => x is JObject y && x["browser"].ToString().Equals(platform)).ToList()[0] as JObject;

            // Get Common Capabilities
            JObject commonCapabilities = config.GetValue("capabilities") as JObject;

            // Merge Capabilities
            capabilities.Merge(commonCapabilities);

            // Get username and accesskey
            string? username = Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME");
            if (username is null)
                username = config.GetValue("user").ToString();

            string? accessKey = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY");
            if (accessKey is null)
                accessKey = config.GetValue("key").ToString();

            capabilities.Add("browserstack.user", username);
            capabilities.Add("browserstack.key", accessKey);

            // Create Desired Cappabilities for WebDriver
            ChromeOptions desiredCapabilities = new ChromeOptions();
            foreach (var x in capabilities)
            {
                desiredCapabilities.AddAdditionalCapability(x.Key, x.Value, true);
            }
            desiredCapabilities.AddAdditionalCapability("name", $"{profile}_test", true);
            
            // Start Local if browserstack.local is set to true
            if (profile.Equals("local") && accessKey is not null)
            {
                desiredCapabilities.AddAdditionalCapability("browserstack.local", true, true);
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

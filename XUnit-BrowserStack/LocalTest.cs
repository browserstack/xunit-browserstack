using Xunit;
using OpenQA.Selenium.Remote;
using System.Text.RegularExpressions;

namespace XUnit_BrowserStack
{
    public class LocalTest : IClassFixture<BaseFixture>
    {

        private readonly BaseFixture baseFixture;

        public LocalTest(BaseFixture baseFixture)
        {
            this.baseFixture = baseFixture;
        }

        [Fact]
        [Trait("profile", "local")]
        public void Test()
        {
            try
            {
                RemoteWebDriver driver = baseFixture.GetDriver("chrome", "local");
                driver.Navigate().GoToUrl("http://bs-local.com:45691/check");
                bool isUpAndRunning = Regex.IsMatch(driver.PageSource, "Up and running", RegexOptions.IgnoreCase);
                Assert.True(isUpAndRunning);
                baseFixture.SetStatus(isUpAndRunning);
            }
            catch (Exception)
            {
                baseFixture.SetStatus(false);
                throw;
            }
        }
    }
}

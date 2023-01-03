using Xunit;
using OpenQA.Selenium.Remote;
using System.Text.RegularExpressions;

namespace XUnit_BrowserStack
{
    public class SampleLocalTest : IClassFixture<BaseFixture>
    {

        private readonly BaseFixture baseFixture;

        public SampleLocalTest(BaseFixture baseFixture)
        {
            this.baseFixture = baseFixture;
        }

        [Fact]
        [Trait("profile", "sample_local_test")]
        public void Test()
        {
            RemoteWebDriver driver = baseFixture.GetDriver("chrome", "local");
            driver.Navigate().GoToUrl("http://bs-local.com:45454/");
            Assert.True(Regex.IsMatch(driver.Title, @"/BrowserStack Local/i", RegexOptions.IgnoreCase));
        }
    }
}

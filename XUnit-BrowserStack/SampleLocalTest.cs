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
        [Trait("Category", "sample-local-test")]
        public void Test()
        {
            RemoteWebDriver driver = baseFixture.GetDriver("chrome", "local");
            driver.Navigate().GoToUrl("http://bs-local.com:45454/");
            Assert.Contains("BrowserStack Local", driver.Title);
        }
    }
}

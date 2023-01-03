using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace XUnit_BrowserStack
{
    public class SampleTest : IClassFixture<BaseFixture>
    {

        private readonly BaseFixture baseFixture;

        public SampleTest(BaseFixture baseFixture)
        {
            this.baseFixture = baseFixture;
        }

        [Fact]
        [Trait("profile", "sample_test")]
        public void Test()
        {
            RemoteWebDriver driver = baseFixture.GetDriver("chrome", "single");
            WebDriverWait webDriverWait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(2000));
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://bstackdemo.com/");
            Assert.Equal("StackDemo", driver.Title);

            string productOnPageText = webDriverWait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"1\"]/p"))).Text;

            webDriverWait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"1\"]/div[4]"))).Click();
            bool cartOpened = webDriverWait.Until(driver => driver.FindElement(By.XPath("//*[@class=\"float-cart__content\"]"))).Displayed;
            Assert.True(cartOpened);
            string productOnCartText = webDriverWait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"__next\"]/div/div/div[2]/div[2]/div[2]/div/div[3]/p[1]"))).Text;
            Assert.Equal(productOnCartText, productOnPageText);
        }
    }
}

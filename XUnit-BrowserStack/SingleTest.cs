using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace XUnit_BrowserStack
{
    public class SingleTest : IClassFixture<BaseFixture>
    {

        private readonly BaseFixture baseFixture;

        public SingleTest(BaseFixture baseFixture)
        {
            this.baseFixture = baseFixture;
        }

        [Fact]
        [Trait("profile", "single")]
        public void Test()
        {
            try
            {
                RemoteWebDriver driver = baseFixture.GetDriver("chrome", "single");
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl("https://bstackdemo.com/");
                Assert.Equal("StackDemo", driver.Title);

                string productOnPageText = driver.FindElement(By.XPath("//*[@id=\"1\"]/p")).Text;

                driver.FindElement(By.XPath("//*[@id=\"1\"]/div[4]")).Click();
                bool cartOpened = driver.FindElement(By.XPath("//*[@class=\"float-cart__content\"]")).Displayed;
                Assert.True(cartOpened);
                string productOnCartText = driver.FindElement(By.XPath("//*[@id=\"__next\"]/div/div/div[2]/div[2]/div[2]/div/div[3]/p[1]")).Text;
                Assert.Equal(productOnCartText, productOnPageText);
                baseFixture.SetStatus(cartOpened && productOnCartText.Equals(productOnPageText));
            }
            catch (Exception)
            {
                baseFixture.SetStatus(false);
                throw;
            }
        }
    }
}

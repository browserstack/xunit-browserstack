using Xunit;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace XUnit_BrowserStack
{
    public abstract class ParallelBaseTest
    {
        private readonly BaseFixture baseFixture;

        public ParallelBaseTest(BaseFixture baseFixture)
        {
            this.baseFixture = baseFixture;
        }

        public void BaseTest(string platform)
        {
            try
            {
                RemoteWebDriver driver = baseFixture.GetDriver(platform, "parallel");
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

    public class ChromeTest : ParallelBaseTest, IClassFixture<BaseFixture>
    {
        public ChromeTest(BaseFixture baseFixture): base(baseFixture) { }

        [Fact]
        [Trait("profile", "parallel")]
        public void Test()
        {
            BaseTest("chrome");
        }
    }

    public class FirefoxTest : ParallelBaseTest, IClassFixture<BaseFixture>
    {
        public FirefoxTest(BaseFixture baseFixture) : base(baseFixture) { }

        [Fact]
        [Trait("profile", "parallel")]
        public void Test()
        {
            BaseTest("firefox");
        }
    }

    public class SafariTest : ParallelBaseTest, IClassFixture<BaseFixture>
    {
        public SafariTest(BaseFixture baseFixture) : base(baseFixture) { }

        [Fact]
        [Trait("profile", "parallel")]
        public void Test()
        {
            BaseTest("safari");
        }
    }

}

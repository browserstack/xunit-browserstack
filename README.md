# xunit-browserstack
[XUnit](https://xunit.net/) Integration with BrowserStack.

<img src="https://github.com/browserstack/cucumber-java-browserstack/blob/master/src/test/resources/img/browserstack.png?raw=true" width="60" height="60" alt="BrowserStack" > <img src="https://xunit.net/images/full-logo.svg" width="150" height="60" alt="XUnit">

## Run Sample Build
* Clone the repo
* Open the solution `NUnit-BrowserStack.sln` in Visual Studio
* Build the solution
* Update `browserstack.yml` file with your [BrowserStack Username and Access Key](https://www.browserstack.com/accounts/settings)
### Running your tests from CLI
* To run the test suite having cross-platform with parallelization, dotnet test --filter "Category=sample-test"
* To run local tests, dotnet test --filter "Category=sample-local-test"
### Running your tests from Test Explorer
- To run a parallel tests, run test with fixture `sample-test`
- To run local tests, run test with fixture `sample-local-test`

 Understand how many parallel sessions you need by using our [Parallel Test Calculator](https://www.browserstack.com/automate/parallel-calculator?ref=github)

 ## Integrate your test suite

This repository uses the BrowserStack SDK to run tests on BrowserStack. Follow the steps below to install the SDK in your test suite and run tests on BrowserStack:

* Create sample browserstack.yml file with the browserstack related capabilities with your [BrowserStack Username and Access Key](https://www.browserstack.com/accounts/settings) and place it in your root folder.
* Add nuget library BrowserStack.TestAdapter
```sh
dotnet add BrowserStack.TestAdapter
```
* Build project `dotnet build`

## Notes
* You can view your test results on the [BrowserStack Automate dashboard](https://www.browserstack.com/automate)
* To test on a different set of browsers, check out our [platform configurator](https://www.browserstack.com/automate/c-sharp#setting-os-and-browser)
* You can export the environment variables for the Username and Access Key of your BrowserStack account. 

  * For Unix-like or Mac machines:
  ```
  export BROWSERSTACK_USERNAME=<browserstack-username> &&
  export BROWSERSTACK_ACCESS_KEY=<browserstack-access-key>
  ```

  * For Windows:
  ```
  set BROWSERSTACK_USERNAME=<browserstack-username>
  set BROWSERSTACK_ACCESS_KEY=<browserstack-access-key>
  ```

## Addtional Resources
* [Documentation for writing Automate test scripts in C#](https://www.browserstack.com/docs/automate/selenium/getting-started/c-sharp)
* [Customizing your tests on BrowserStack](https://www.browserstack.com/automate/capabilities)
* [Browsers & mobile devices for selenium testing on BrowserStack](https://www.browserstack.com/list-of-browsers-and-platforms?product=automate)
* [Using REST API to access information about your tests via the command-line interface](https://www.browserstack.com/automate/rest-api)

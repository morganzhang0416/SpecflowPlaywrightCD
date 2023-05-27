# Test Automation UI Project using Playwright and Specflow

## Summary
Test Automation Sample Project using Playwright and Specflow




## Dev Instructions

Once this project has been cloned the dependencies will need to be downloaded by executing this command in the project root:

```bash
dotnet restore
```

Update the following in `appsettings`

- url - Application Url to run tests against

Ensure the proper browser drivers(s) is(are) downloaded locally: see below.

Build the tests project

```bash
dotnet build
```

Run the tests by executing:

```bash
dotnet test
```


## Living docs

Generate test report on living doc from feature-folder
```bash
livingdoc feature-folder D:\TestProject\SpecflowPlaywrightCD\src\PlaywrightCD.Tests.UI -t  D:\TestProject\SpecflowPlaywrightCD\src\PlaywrightCD.Tests.UI\bin\Debug\net6.0\TestExecution.json
```

Generate test report on living doc from Test-assembly
```bash
livingdoc test-assembly D:\TestProject\SpecflowPlaywrightCD\src\PlaywrightCD.Tests.UI\bin\Debug\net6.0\PlaywrightCD.Tests.UI.dll -t  D:\TestProject\SpecflowPlaywrightCD\src\PlaywrightCD.Tests.UI\bin\Debug\net6.0\TestExecution.json
```



## Support

Please create any support requests via a properly labeled issues in this repository.

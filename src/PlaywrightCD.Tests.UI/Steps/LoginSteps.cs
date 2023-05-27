using System;
using FluentAssertions;
using NUnit.Framework;
using PlaywrightCD.Models;
using PlaywrightCD.Pages;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using PlaywrightCD.Utils;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;


namespace PlaywrightCD.Tests.UI
{
    [Binding]
    public class LoginSteps
    {
        private readonly LoginPage _loginPage;
        private readonly PageFunctions _pagefunctions;
        public LoginSteps(LoginPage homepage, PageFunctions pagefunctions)
        {
            _loginPage = homepage;
            _pagefunctions = pagefunctions;
        }

        [Given(@"I navigate to the website sign in page")]
        public async Task GivenINavigateToTheWebsiteSignInPage()
        {
            //await _pagefunctions.RandomWait(1000, 3000);
            await _loginPage.GoTo();
        }


        [When(@"I login with a valid users credentials (.*) (.*)")]
        public async Task WhenILoginWithAValidUsersCredentials(string userName, string password)
        {
            await _loginPage.Login(userName, password);
        }

        [When(@"I login with users credentials")]
        public async Task WhenILoginWithUsersCredentials(Table table)
        {
            var testData = table.CreateInstance<User>();
            await _loginPage.Login(testData.UserName, testData.Password);
        }

        [Then(@"the user should be logged in successfully")]
        public async Task ThenTheUserShouldBeLoggedInSuccessfully()
        {
           //Delay Task wait for Login completed
            await Task.Delay(3000);
            //reload page as it did not reponsed after click sign button at some times
            await _pagefunctions.ReloadPage();
         

            var actualSuccessMessage = await _loginPage.GetSuccessMessage();
            actualSuccessMessage.Should().Contain("Kia ora");


        }

        [Then(@"the user should not be logged in")]
        public async Task ThenTheUserShouldNotBeLoggedIn()
        {
            var actualErrorMessage = await _loginPage.GetErrorMessage();
            actualErrorMessage.Should().Be("An invalid email and/or password has been entered. Please try again. Please note, passwords are case-sensitive.");
        }

        [Given(@"I am on the registration page")]
        public async Task GivenIAmOnTheRegistrationPage()
        {
            await _loginPage.RegistrationGoTo();
        }

        [When(@"I enter eamil and password details")]
        public async Task WhenIEnterEamilAndPasswordDetails()
        {
            await _loginPage.CreateAnAccount();
        }

        [When(@"I click on the add you details button")]
        public async Task WhenIClickOnTheAddYouDetailsButton()
        {
            await _loginPage.ClickAddYourDetailsButton();
        }

        [Then(@"I should see add your details page")]
        public async Task ThenIShouldSeeAddYourDetailsPage()
        {
            await _loginPage.AssertAddYourDetails();
        }

        [When(@"I enter valid details on add your details page")]
        public async Task WhenIEnterValidDetailsOnAddYourDetailsPage()
        {
            await _loginPage.AddYourDetails();
        }

        [When(@"I click on submit button")]
        public async Task WhenIClickOnSubmitButton()
        {
            await _loginPage.ClickSubmitButton();
        }

        [Then(@"I should see welcome message")]
        public async Task ThenIShouldSeeWelcomeMessage()
        {
            await _loginPage.AssertRegistrationSuccess();
        }

    }
}

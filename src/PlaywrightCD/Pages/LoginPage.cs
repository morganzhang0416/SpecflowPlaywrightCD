using System;
using Microsoft.Playwright;
using System.Threading.Tasks;
using Practice.One.UI.Settings;

namespace PlaywrightCD.Pages
{
    public class LoginPage
    {
        private readonly IPage _page;
        
        public LoginPage(IPage page)
        {
            _page = page;
        }
     
        private async Task ClickSignLink() => await _page.ClickAsync("button:text('Sign in or Register')");
        private async Task ClickSignButton() => await _page.ClickAsync("button:text('Sign in')");
        private async Task WaitForUsername() => await _page.WaitForSelectorAsync("[id=loginID]");
        private async Task EnterUserName(string userName) => await _page.TypeAsync("[id=loginID]", userName);
        private async Task EnterPassword(string password) => await _page.TypeAsync("[id=password]", password);
        private async Task ClickLoginButton() => await _page.ClickAsync("button[value='Submit']:has-text('Sign in')");
        private async Task WaitForEmailReg() => await _page.WaitForSelectorAsync("[id=email]");
        private async Task EnterEmailReg(string emailReg) => await _page.TypeAsync("[id=email]", emailReg);
        private async Task EnterPasswordReg(string passwordReg) => await _page.TypeAsync("[data-cy=password]", passwordReg);
        public async Task ClickAddYourDetailsButton() => await _page.ClickAsync("button:text('Add your details')");
        private async Task EnterFirstName(string firstname) => await _page.TypeAsync("[id=firstName]", firstname);
        private async Task EnterLastName(string lastname) => await _page.TypeAsync("[id=lastName]", lastname);
        private async Task EnterStreetAddress(string streetAddress) => await _page.TypeAsync("[data-cy=addressStreet1]", streetAddress);
        private async Task EnterPhoneNumber(string phoneNumber) => await _page.TypeAsync("[id=phoneNumber]", phoneNumber);
        private async Task EnterDayDOB(string dobDay) => await _page.TypeAsync("[id=dobDay]", dobDay);
        private async Task EnterMonthDOB(string dobMonth) => await _page.TypeAsync("[id=dobMonth]", dobMonth);
        private async Task EnterYearDOB(string dobYear) => await _page.TypeAsync("[id=dobYear]", dobYear);
        private async Task EnterLoyaltyCard(string loyaltyCard) => await _page.TypeAsync("[id=loyaltyCard]", loyaltyCard);
        private async Task CheckReceiveEmail() => await _page.CheckAsync("#doesWishToReceiveOffers");
        public async Task ClickSubmitButton() => await _page.ClickAsync("button:text('Submit')");
        public async Task PressArrowDownKey() => await _page.PressAsync("[data-cy = addressStreet1]", "ArrowDown");
        public async Task PressEnterKey() => await _page.PressAsync("[data-cy = addressStreet1]", "Enter");

        /// <summary>
        /// Navigates user to the application HomePage.
        /// </summary>
        /// <returns></returns>
        public async Task GoTo() => await _page.GotoAsync(ConfigurationService.GetWebSettings().BaseUrl+"shop/securelogin");

        public async Task RegistrationGoTo() => await _page.GotoAsync(ConfigurationService.GetWebSettings().BaseUrl + "shop/register");


        /// <summary>
        /// Login to the application with a user.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        ///  /// <returns></returns>
        public async Task Login(string userName, string password)
        {
            await WaitForUsername();
            await EnterUserName(userName);
            await EnterPassword(password);
            await ClickLoginButton();
        }

        

        /// <summary>
        /// Gets the success and error message text.
        /// </summary>
        /// <returns></returns>
        /// 
        public async Task<string> GetSuccessMessage()
        {
            string expectedButtonText = "Kia ora";
            return await _page.Locator("button[cdxbutton]:has-text(\"" + expectedButtonText + "\")").InnerTextAsync(); ;

        }

        public async Task<string> GetErrorMessage()
        {
            await _page.WaitForSelectorAsync("id=alertLabel", new PageWaitForSelectorOptions { Timeout = 8000 });
            return await _page.Locator("id=alertLabel").InnerTextAsync();
            
        }

        public async Task CreateAnAccount()
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            await WaitForEmailReg();
            await EnterEmailReg("Alan_" + timestamp + "@gmail.com");
            await EnterPasswordReg("Gogo_123.");
        }


        public async Task AssertAddYourDetails()
        {
            string pageContent = await _page.ContentAsync();

            if (!pageContent.Contains("Add your details"))
            {
                throw new Exception("The page does not contain the expected text.");
            }
        }

        public async Task AddYourDetails()
        {
            
            await EnterFirstName("Tom");
            await EnterLastName("Jackson");
            await EnterStreetAddress("13 Point Chevalier Road, Point Chevalier, Auckland 1022");
            await Task.Delay(3000);
            await PressArrowDownKey();
            await PressEnterKey();
            await EnterPhoneNumber("0211866123");
            await EnterDayDOB("01");
            await EnterMonthDOB("01");
            await EnterYearDOB("1982");
            await EnterLoyaltyCard("9480002412345");
            //await CheckReceiveEmail();

        }

        public async Task AssertRegistrationSuccess()
        {
            string pageContent = await _page.ContentAsync();

            if (!pageContent.Contains("You're all set and ready to shop online!"))
            {
                throw new Exception("The page does not contain the expected text.");
            }
        }

    }
}

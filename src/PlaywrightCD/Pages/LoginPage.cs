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

        /// <summary>
        /// Navigates user to the application HomePage.
        /// </summary>
        /// <returns></returns>
        public async Task GoTo() => await _page.GotoAsync(ConfigurationService.GetWebSettings().BaseUrl+"/shop/securelogin");


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
    }
}

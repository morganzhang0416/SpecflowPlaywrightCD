using Microsoft.Playwright;
using System.Threading.Tasks;
using Practice.One.UI.Settings;
using System.Text.RegularExpressions;
using NUnit.Framework;
using System;
using System.Diagnostics;


namespace PlaywrightCD.Pages
{
    public class ShopPage
    {
        private readonly IPage _page;
        
        public ShopPage(IPage page)
        {
            _page = page;
        }
     
       
        private async Task WaitForSearchBox() => await _page.WaitForSelectorAsync("[id=search]");
        private async Task EnterItemForSearch(string item) => await _page.TypeAsync("[id=search]", item);
        private async Task HitEnterKey() =>  await _page.Keyboard.PressAsync("Enter");

        /// <summary>
        /// Navigates user to the application HomePage.
        /// </summary>
        /// <returns></returns>
        public async Task GoTo() => await _page.GotoAsync(ConfigurationService.GetWebSettings().BaseUrl);


        public async Task SearchItem(string item)
        {
            _page.Dialog += (_, dialog) => dialog.AcceptAsync();
            await _page.ClickAsync("button:has-text('Got it')");
            await WaitForSearchBox();
            await EnterItemForSearch(item);
            await HitEnterKey();
           
        }

        /// <summary>
        /// Gets the error message text.
        /// </summary>
        /// <returns></returns>
        /// 

        public async Task AssertProductTitlesContainText(string text)
        {
            Regex regex = new Regex("^product-\\d+-title$");
            var elements = await _page.QuerySelectorAllAsync($"[id^='product'][id$='-title']");

            foreach (var titleElement in elements)
            {
                string title = await titleElement.InnerTextAsync();
                Assert.IsTrue(title.ToLower().Contains(text.ToLower()));
                //System.Console.WriteLine("title" + title.ToLower());
                //System.Console.WriteLine("text" + text.ToLower());
            }
        }
    }
}

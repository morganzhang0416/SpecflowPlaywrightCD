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
        public async Task ClickTrolley() => await _page.ClickAsync("a[href=\"/reviewtrolley\"]");



        /// <summary>
        /// Navigates user to the application HomePage.
        /// </summary>
        /// <returns></returns>
        public async Task GoTo() => await _page.GotoAsync(ConfigurationService.GetWebSettings().BaseUrl);

        //this is old method, might delete in future
        public async Task SearchItemBak(string item)
        {
            _page.Dialog += (_, dialog) => dialog.AcceptAsync();
            await _page.ClickAsync("button:has-text('Got it')");
            await WaitForSearchBox();
            await EnterItemForSearch(item);
            await HitEnterKey();
           
        }

        public async Task SearchItem(string item)
        {
            var dialogTask = new TaskCompletionSource<bool>();
            _page.Dialog += (_, dialog) =>
            {
                dialog.AcceptAsync();
                dialogTask.TrySetResult(true);
            };

            if (await Task.WhenAny(dialogTask.Task, Task.Delay(3000)) == dialogTask.Task)
            {
                // If a dialog appeared within 3 seconds
                try
                {
                    await _page.ClickAsync("button:has-text('Got it')");
                }
                catch (PlaywrightException)
                {
                    // The "Got it" button was not found, handle exception here if necessary.
                }
            }

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

        // add the second item to cart

        public async Task AddSecondItemToCart()
        {
            try
            {
                await _page.WaitForSelectorAsync(".addToTrolley-btn--add");
                var buttonElements = await _page.QuerySelectorAllAsync(".addToTrolley-btn--add");
                Console.WriteLine(buttonElements);


                //var buttonElements = await _page.QuerySelectorAllAsync("[data-cy='addToTrolleyBtn']");
                if (buttonElements.Count > 1)
                {
                    await buttonElements[1].ClickAsync();
                }
                else
                {
                    throw new Exception("Not enough elements found with the class 'addToTrolley-btn--add'");
                }
                Console.WriteLine("Number of elements found: " + buttonElements.Count);

            }
            catch (PlaywrightException)
            {
                throw new Exception("No elements found with the text 'Add to trolley'");
            }
        }



        public async Task VerifySignInAlert()
        {
            var alertElement = await _page.WaitForSelectorAsync("#dialog-title");
            string alertText = await alertElement.InnerTextAsync();
            Assert.AreEqual("Sign in to add items to your trolley.", alertText);
        }

        public async Task AddFirstItemToCartAndCheckCart()
        {
            try
            {

                await _page.ClickAsync("text=Add to trolley");
            }
            catch (PlaywrightException)
            {
                throw new Exception("No elements found with the text 'Add to trolley'");
            }
        }

    }
}

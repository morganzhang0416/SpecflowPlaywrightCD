using Microsoft.Playwright;
using System.Threading.Tasks;
using Practice.One.UI.Settings;
using System.Text.RegularExpressions;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Linq;



namespace PlaywrightCD.Pages
{
    public class ShopPage
    {
        private readonly IPage _page;
        private string itemTitle;
        private string itemTitleID;
        private string itemPrice;
        private string itemPriceID;


        public ShopPage(IPage page)
        {
            _page = page;
        }
     
       
        private async Task WaitForSearchBox() => await _page.WaitForSelectorAsync("[id=search]");
        private async Task EnterItemForSearch(string item) => await _page.TypeAsync("[id=search]", item);
        private async Task HitEnterKey() =>  await _page.Keyboard.PressAsync("Enter");
        public async Task ClickClearTrolley() => await _page.ClickAsync("#clearTrolleyButton");
        public async Task WaiclearTrolleyConfirm() => await _page.WaitForSelectorAsync(".clearTrolley-yes");
        public async Task ClickClearTrolleyConfirm() => await _page.ClickAsync(".clearTrolley-yes");
       




        /// <summary>
        /// Navigates user to the application HomePage.
        /// </summary>
        /// <returns></returns>
        public async Task GoTo() => await _page.GotoAsync(ConfigurationService.GetWebSettings().BaseUrl);

        public async Task SearchGoTo(String item) => await _page.GotoAsync(ConfigurationService.GetWebSettings().BaseUrl + "shop/searchproducts?search="+item);
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

        public async Task AddItemToCart(int i)
        {
            try
            {
                await _page.WaitForSelectorAsync(".addToTrolley-btn--add");
                var buttonElements = await _page.QuerySelectorAllAsync(".addToTrolley-btn--add");
               


                //var buttonElements = await _page.QuerySelectorAllAsync("[data-cy='addToTrolleyBtn']");
                if (buttonElements.Count > i-1)
                {
                    await buttonElements[i-1].ClickAsync();
                   
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

        public async Task GetItemName(int i)
        {
            try
            {
               
                Regex regex = new Regex("^product-\\d+-title$");
                var SKUElements = await _page.QuerySelectorAllAsync($"[id^='product'][id$='-title']");
                //Console.WriteLine(buttonElements);


                //var buttonElements = await _page.QuerySelectorAllAsync("[data-cy='addToTrolleyBtn']");
                if (SKUElements.Count > i - 1)
                {
                    itemTitle = await SKUElements[i - 1].InnerTextAsync();
                    itemTitleID = await SKUElements[i - 1].GetAttributeAsync("id");
                }
                else
                {
                    throw new Exception("Not enough elements found with the id product-xxxxxx-title");
                }
                Console.WriteLine("Number of elements found: " + SKUElements.Count);

            }
            catch (PlaywrightException)
            {
                throw new Exception("No elements found with the text 'product-xxxxxx-title'");
            }
        }

        public async Task GetItemPrice(int i)
        {
            try
            {

                Regex regex = new Regex("^product-\\d+-price$");
                var SKUElements = await _page.QuerySelectorAllAsync($"[id^='product'][id$='-price']");
                //Console.WriteLine(buttonElements);


                //var buttonElements = await _page.QuerySelectorAllAsync("[data-cy='addToTrolleyBtn']");
                if (SKUElements.Count > i - 1)
                {
                    itemPrice = await SKUElements[i - 1].GetAttributeAsync("aria-label");
                    itemPriceID = await SKUElements[i - 1].GetAttributeAsync("id");
                }
                else
                {
                    throw new Exception("Not enough elements found with the id product-xxxxxx-price");
                }
                Console.WriteLine("Number of elements found: " + SKUElements.Count);

            }
            catch (PlaywrightException)
            {
                throw new Exception("No elements found with the text 'product-xxxxxx-title'");
            }
        }


        

        public async Task VerifySignInAlert()
        {
            var alertElement = await _page.WaitForSelectorAsync("#dialog-title");
            string alertText = await alertElement.InnerTextAsync();
            Assert.AreEqual("Sign in to add items to your trolley.", alertText);
        }

        public async Task ClickTrolley()
        {
            try
            {
               
                var elements = await _page.QuerySelectorAllAsync("global-nav-basket-totals a");
                await elements[elements.Count - 1].ClickAsync();

            }
            catch (PlaywrightException)
            {
                throw new Exception("No <a> element found inside <global-nav-basket-totals>");
            }
        }

        public async Task CheckTrolley()
        {
            try
            {
                await _page.WaitForSelectorAsync("#" + itemTitleID);
                var nameElement = await _page.QuerySelectorAsync("#"+ itemTitleID); // Using CSS selector to target by id
                var priceElement = await _page.QuerySelectorAsync("#" + itemPriceID);
                var QuantityElements = await _page.QuerySelectorAllAsync($"[id^='quantity']");
                string inputValue = await QuantityElements[0].GetAttributeAsync("value");
                Console.WriteLine("Input Value: " + inputValue);

                if (nameElement != null& priceElement != null)
                {
                    string nameInCartText = await nameElement.InnerTextAsync();
                    string PriceInCartText = await priceElement.GetAttributeAsync("aria-label");
                    Console.WriteLine("Element text: " + nameInCartText);
                    Assert.IsTrue(nameInCartText.ToLower().Contains(itemTitle.ToLower()));
                    Assert.IsTrue(PriceInCartText.ToLower().Contains(itemPrice.ToLower()));
                }
                else
                {
                    throw new Exception("No element found with the id provided");
                }
            }
            catch (PlaywrightException)
            {
                throw new Exception("No element found with the id provided ");
            }
        }

        public async Task AssertNoItemsInTrolley()
        {
            await _page.WaitForSelectorAsync("[data-cy='totalItems']");
            var element = await _page.QuerySelectorAsync("[data-cy='totalItems']");
            var text = await element.InnerTextAsync();

            Assert.AreEqual("No items", text);
        }

        public async Task SortByCondition(String value)
        {
            await _page.WaitForSelectorAsync("#sortby-dropdown-0");
            await _page.SelectOptionAsync("#sortby-dropdown-0","PriceAsc");
            await _page.WaitForLoadStateAsync();

        }

        public async Task AssertSortedResult()
        {
            await Task.Delay(3000);
            var priceElements = await _page.QuerySelectorAllAsync($"[id^='product'][id$='-price']");

            System.Collections.Generic.List<string> priceStrings = new System.Collections.Generic.List<string>();

            //List<string> priceStrings = new List<string>();
            foreach (var priceElement in priceElements)
            {
                var priceString = await priceElement.GetAttributeAsync("aria-label");
                priceStrings.Add(priceString);
            }

            System.Collections.Generic.List<double> prices = priceStrings.Select(priceString => ParsePrice(priceString)).ToList();

            //List<double> prices = priceStrings.Select(priceString => ParsePrice(priceString)).ToList();

            for (int i = 1; i < prices.Count; i++)
            {
                if (prices[i] < prices[i - 1])
                {
                    throw new Exception("Prices are not sorted from low to high");
                }
            }
        }

        private double ParsePrice(string priceString)
        {
            string pattern = @"\d+\.\d+";

            Match match = Regex.Match(priceString, pattern);
            if (match.Success)
            {
                string value = match.Value;
                Console.WriteLine(value);
                double price = Convert.ToDouble(value);
                return price;
            }
            else
            {
                throw new Exception("Price string does not contain a numeric value.");
            }
        }


    }
}

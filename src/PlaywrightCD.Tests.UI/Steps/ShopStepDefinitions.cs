using System;
using TechTalk.SpecFlow;
using FluentAssertions;
using NUnit.Framework;
using PlaywrightCD.Models;
using PlaywrightCD.Pages;
using System.Threading.Tasks;
using TechTalk.SpecFlow.Assist;
using PlaywrightCD.Utils;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;
using static System.Net.Mime.MediaTypeNames;

namespace PlaywrightCD.Tests.UI
{
    [Binding]
    public class ShopStepDefinitions
    {
        private readonly PageFunctions _pagefunctions;
        private readonly ShopPage _shopPage;
        public ShopStepDefinitions(PageFunctions pageFunctions, ShopPage shopPage)
        {
            _pagefunctions = pageFunctions;
            _shopPage = shopPage;
        }

        [Given(@"I navigate to the website home page")]
        public async Task GivenINavigateToTheWebsiteHomePage()
        {
            await _shopPage.GoTo();
        }


        [When(@"I type keyword (.*) in search box")]
        public async Task WhenITypeKeywordMilkInSearchBox(String item)
        {
            await _shopPage.SearchItem(item);
        }


        [Then(@"Eeach reuslt should  include keyword (.*)")]
        public  async Task ThenEeachReusltShouldIncludeKeyword(String item)
        {
            await _shopPage.AssertProductTitlesContainText(item);

        }

        [When(@"I click Add to trolley on the (.*) search result")]
        public async Task WhenIClickAddToTrolleyOnTheSearchResult(int i)
        {
            await _shopPage.AddItemToCart(i);
            await _shopPage.GetItemName(i);
            await _shopPage.GetItemPrice(i);
        }



        [Then(@"I should see sign in alert")]
        public async Task ThenIShouldSeeSignInAlert()
        {
            await _shopPage.VerifySignInAlert();
        }

        [When(@"I click trolley button")]
        public async Task WhenIClickTrolleyButton()
        {
            
            await _shopPage.ClickTrolley();
        }

        [Then(@"I should see item added")]
        public async Task ThenIShouldSeeItemAdded()
        {
            await _shopPage.CheckTrolley();
        }

        [When(@"I click clearTrolleyButton")]
        public async Task WhenIClickClearTrolleyButton()
        {
            await _shopPage.ClickClearTrolley();
            await _shopPage.WaiclearTrolleyConfirm();
            await _shopPage.ClickClearTrolleyConfirm();
        }

        [Then(@"I should see no items in trolley")]
        public async Task ThenIShouldSeeNoItemsInTrolley()
        {
            await _shopPage.AssertNoItemsInTrolley();
        }

        [Given(@"I have searched for ""([^""]*)""")]
        public async Task GivenIHaveSearchedFor(string item)
        {
            await _shopPage.SearchGoTo(item);
        }

        [When(@"I sort the results by ""([^""]*)""")]
        public async Task WhenISortTheResultsBy(string value)
        {
            await _shopPage.SortByCondition(value);
        }

        [Then(@"I should see the milk sorted from ""([^""]*)""")]
        public async Task ThenIShouldSeeTheMilkSortedFrom(string value)
        {
            await _shopPage.AssertSortedResult();
        }


    }
}

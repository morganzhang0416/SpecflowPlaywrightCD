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

        [When(@"I tpype keyword (.*) in search box")]
        public async Task WhenITpypeKeywordInSearchBox(String item)
        {
            await _shopPage.SearchItem(item);

        }

        [Then(@"Eeach reuslt should  include keyword (.*)")]
        public  async Task ThenEeachReusltShouldIncludeKeyword(String item)
        {
            await _shopPage.AssertProductTitlesContainText(item);

        }
    }
}

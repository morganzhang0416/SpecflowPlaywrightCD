using System;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace PlaywrightCD.Utils
{
    public class PageFunctions
    {
        private readonly IPage _page;
        private static readonly Random random = new Random();

        public PageFunctions(IPage page)
        {
            _page = page;
        }

        public async Task ReloadPage() => await _page.ReloadAsync();

        public async Task<bool> PageContainsText(string searchText)
        {
            var textLocator = _page.GetByText(searchText);
            string pageText = await textLocator.EvaluateAsync<string>("e => e.innerText");
            return pageText.Contains(searchText);
        }

        

        public  async Task RandomWait(int minWaitTime = 500, int maxWaitTime = 2000)
        {
            // Generate a random wait time between the minimum and maximum values
            int waitTime = random.Next(minWaitTime, maxWaitTime);

            // Wait for the random duration
            await Task.Delay(waitTime);
        }
    }

}

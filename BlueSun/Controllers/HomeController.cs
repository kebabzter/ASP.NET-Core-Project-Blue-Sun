namespace BlueSun.Controllers
{
    using BlueSun.Models;
    using BlueSun.Services.NFTCollections;
    using BlueSun.Services.NFTCollections.Models;
    using BlueSun.Services.Statistics;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using System.Diagnostics;
    public class HomeController : Controller
    {
        private readonly INFTCollectionService nftCollections;
        private readonly IMemoryCache cache;

        public HomeController(
            INFTCollectionService nftCollections,
            IMemoryCache cache)
        {
            this.nftCollections = nftCollections;
            this.cache = cache;
        }

        public IActionResult Index()
        {
            const string latestNFTCollectionsCacheKey = "LatestNFTCollectionsCacheKey";

            var latestNftCollections = this.cache.Get<List<LatestNFTCollectionServiceModel>>(latestNFTCollectionsCacheKey);

            if (latestNftCollections == null)
            {
                latestNftCollections = this.nftCollections
                    .Latest()
                    .ToList();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

                this.cache.Set(latestNFTCollectionsCacheKey, latestNftCollections, cacheOptions);
            }



            return View(latestNftCollections);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
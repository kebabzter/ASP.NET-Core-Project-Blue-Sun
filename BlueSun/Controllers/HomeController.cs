namespace BlueSun.Controllers
{
    using BlueSun.Data;
    using BlueSun.Models;
    using BlueSun.Models.Home;
    using BlueSun.Models.NFTCollections;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    public class HomeController : Controller
    {
        private readonly BlueSunDbContext data;

        public HomeController(BlueSunDbContext data) => this.data = data;

        public IActionResult Index()
        {
            var totalNFTCollections = this.data.NFTCollections.Count();
            var totalNFTs = this.data.NFTs.Count();

            var nftCollections = this.data
                .NFTCollections
                .OrderByDescending(n => n.Id)
                .Select(n => new NFTCollectionIndexViewModel
                {
                    Id = n.Id,
                    Name = n.Name,
                    ImageUrl = n.ImageUrl,
                })
                .Take(3)
                .ToList();

            return View(new IndexViewModel
            {
                TotalNFTCollections = totalNFTCollections,
                TotalNFTs = totalNFTs,
                NFTCollections = nftCollections
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
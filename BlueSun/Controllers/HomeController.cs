namespace BlueSun.Controllers
{
    using BlueSun.Data;
    using BlueSun.Models;
    using BlueSun.Models.Home;
    using BlueSun.Models.NFTCollections;
    using BlueSun.Services.Statistics;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    public class HomeController : Controller
    {
        private readonly IStatisticsService statistics;
        private readonly BlueSunDbContext data;

        public HomeController(
            IStatisticsService statistics,
            BlueSunDbContext data)
        {
            this.statistics = statistics;
            this.data = data;
        }

        public IActionResult Index()
        {
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

            var totalStatistics = this.statistics.Total();

            return View(new IndexViewModel
            {
                TotalNFTCollections = totalStatistics.TotalNFTCollections,
                TotalUsers = totalStatistics.TotalUsers,
                TotalNFTs = totalStatistics.TotalNFTs,
                NFTCollections = nftCollections
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
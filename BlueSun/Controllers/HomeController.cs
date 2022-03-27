namespace BlueSun.Controllers
{
    using AutoMapper;
    using BlueSun.Data;
    using BlueSun.Models;
    using BlueSun.Models.Home;
    using BlueSun.Services.NFTCollections;
    using BlueSun.Services.Statistics;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    public class HomeController : Controller
    {
        private readonly INFTCollectionService nftCollections;
        private readonly IStatisticsService statistics;

        public HomeController(
            IStatisticsService statistics,
            INFTCollectionService nftCollections)
        {
            this.statistics = statistics;
            this.nftCollections = nftCollections;
        }

        public IActionResult Index()
        {
            var latestNftCollections = this.nftCollections
                .Latest()
                .ToList();

            var totalStatistics = this.statistics.Total();

            return View(new IndexViewModel
            {
                TotalNFTCollections = totalStatistics.TotalNFTCollections,
                TotalUsers = totalStatistics.TotalUsers,
                TotalNFTs = totalStatistics.TotalNFTs,
                NFTCollections = latestNftCollections
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
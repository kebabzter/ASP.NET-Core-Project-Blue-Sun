namespace BlueSun.Test.Controller
{
    using BlueSun.Controllers;
    using BlueSun.Data.Models;
    using BlueSun.Models.Home;
    using BlueSun.Services.NFTCollections;
    using BlueSun.Services.Statistics;
    using BlueSun.Test.Mocks;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using MyTested.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    public class HomeControllerTest
    {
        // Integration test (doesn't fake things, but uses them as they are)
        [Fact]
        public void IndexShouldReturnViewWithCorrectModelAndData()
            => MyMvc
            .Pipeline()
            .ShouldMap("/")
            .To<HomeController>(c => c.Index())
            .Which(controller => controller
                .WithData(GetNFTCollections()))
            .ShouldReturn()
            .View(view => view
                .WithModelOfType<IndexViewModel>()
                .Passing(m => m.NFTCollections.Should().HaveCount(3)));

        [Fact]
        public void IndexShouldReturnViewWithCorrectModel()
        {
            // Arrange
            var data = DatabaseMock.Instance;
            var mapper = MapperMock.Instance;

            var nftCollections = Enumerable
                .Range(0, 10)
                .Select(i => new NFTCollection { Description = "", ImageUrl = "", Name = "" });

            var nfts = Enumerable
                .Range(0, 5)
                .Select(i => new NFT { Description = "", ImageUrl = "", Name = "" });

            data.NFTs
                .AddRange(nfts);

            data.NFTCollections
                .AddRange(nftCollections);

            data.Users.Add(new User { FullName = ""});

            data.SaveChanges();

            var nftCollectionService = new NFTCollectionService(data,mapper);
            var statisticsService = new StatisticsService(data);

            var homeController = new HomeController(statisticsService, nftCollectionService);

            //Act
            var result = homeController.Index();

            //Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = viewResult.Model;

            var indexViewModel = Assert.IsType<IndexViewModel>(model);

            Assert.Equal(3, indexViewModel.NFTCollections.Count);
            Assert.Equal(5, indexViewModel.TotalNFTs);
            Assert.Equal(10, indexViewModel.TotalNFTCollections);
            Assert.Equal(1, indexViewModel.TotalUsers);
        }

        //[Fact]
        //public void ErrorShouldReturnView()
        //{
        //    // Arrange
        //    var data = DatabaseMock.Instance;
        //    var mapper = MapperMock.Instance;

        //    var nftCollectionService = new NFTCollectionService(data, mapper);
        //    var statisticsService = new StatisticsService(data);

        //    var homeController = new HomeController(statisticsService, nftCollectionService);

        //    //// Act
        //    var result = homeController.Error();

        //    //// Assert
        //    Assert.NotNull(result);
        //    Assert.IsType<ViewResult>(result);
        //}

        [Fact]
        public void ErrorShouldReturnView()
            => MyMvc
            .Pipeline()
            .ShouldMap("/Home/Error")
            .To<HomeController>(t => t.Error())
            .Which()
            .ShouldReturn()
            .View();

        private static IEnumerable<NFTCollection> GetNFTCollections()
            => Enumerable
                .Range(0, 10)
                .Select(i => new NFTCollection { Description = "", ImageUrl = "", Name = "" });
    }
}

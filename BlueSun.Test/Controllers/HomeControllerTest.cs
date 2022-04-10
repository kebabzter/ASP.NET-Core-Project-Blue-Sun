namespace BlueSun.Test.Controller
{
    using BlueSun.Controllers;
    using BlueSun.Services.NFTCollections;
    using BlueSun.Services.NFTCollections.Models;
    using BlueSun.Test.Mocks;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using MyTested.AspNetCore.Mvc;
    using System.Collections.Generic;
    using Xunit;

    using static Data.NFTCollections;
    using static WebConstants.Cache;
    public class HomeControllerTest
    {
        //Integration test(doesn't fake things, but uses them as they are)


       [Fact]
        public void ErrorShouldReturnView()
        {
            var cache = CacheMock.Instance();

            var data = DatabaseMock.Instance;

            var mapper = MapperMock.Instance;

            var nftCollectionsService = new NFTCollectionService(data, mapper);

            var homeController = new HomeController(nftCollectionsService, cache);

            var result = homeController.Error();

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void IndexShouldReturnView()
        {
            var cache = CacheMock.Instance();

            var data = DatabaseMock.Instance;

            var mapper = MapperMock.Instance;

            var nftCollectionsService = new NFTCollectionService(data, mapper);

            var homeController = new HomeController(nftCollectionsService,cache);

            var result = homeController.Index();

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

    }
}

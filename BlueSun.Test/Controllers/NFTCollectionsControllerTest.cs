namespace BlueSun.Test.Controllers
{
    using BlueSun.Controllers;
    using BlueSun.Data.Models;
    using BlueSun.Models.NFTCollections;
    using BlueSun.Services.Artists;
    using BlueSun.Services.NFTCollections;
    using BlueSun.Services.NFTCollections.Models;
    using BlueSun.Services.Users;
    using BlueSun.Test.Mocks;
    using BlueSun.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;
    using Microsoft.AspNetCore.Http;

    public class NFTCollectionsControllerTest
    {
        [Fact]
        public void AllShouldReturnView()
        {
            var data = DatabaseMock.Instance;

            var mapper = MapperMock.Instance;

            var collectionsService = new NFTCollectionService(data, mapper);

            var artistsService = new ArtistService(data);

            var userService = new UserService(data);

            var collectionsController = new NFTCollectionsController(collectionsService, artistsService, mapper, userService, null);

            var nftCollections = new List<NFTCollectionServiceModel>();

            var categories = new List<NFTCollectionCategoryServiceModel>();

            var model = new AllNFTCollectionsQueryModel
            {
                Collections = nftCollections,
                CurrentPage = 0,
                TotalCollections = 0,
                Sorting = CollectionSorting.DateCreated,
                SearchTerm = "",
                Categories = categories,
                Category = ""
            };

            var result = collectionsController.All(model);

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void DetailsShouldReturnView()
        {
            var collection = new NFTCollection
            {
                Name = "TestName",
                ImageUrl = "TestImageUrl",
                Description = "TestDescription",
                NFTs = new List<NFT>(),
                ArtistId = 0,
                CategoryId = 0,
                IsPublic = true
            };

            var data = DatabaseMock.Instance;

            var mapper = MapperMock.Instance;
            var collectionsService = new NFTCollectionService(data, mapper);
            var artistsService = new ArtistService(data);
            var userService = new UserService(data);

            var information = collectionsService.Details(collection.Id).GetInformation();

            var collectionsController = new NFTCollectionsController(collectionsService, artistsService, mapper, userService, null);

            var result = collectionsController.Details(collection.Id, information);

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);

        }

        [Fact]
        public void DetailsShouldReturnBadRequestWhenCollectionIsInvalid()
        {
            var data = DatabaseMock.Instance;

            var mapper = MapperMock.Instance;
            var collectionsService = new NFTCollectionService(data, mapper);
            var artistsService = new ArtistService(data);
            var userService = new UserService(data);

            var collectionsController = new NFTCollectionsController(collectionsService, artistsService, mapper, userService, null);

            var result = collectionsController.Details(1, null);

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void DetailsShouldReturnBadRequestWhenCollectionInformationIsInvalid()
        {
            var collection = new NFTCollection
            {
                Name = "TestName",
                ImageUrl = "TestImageUrl",
                Description = "TestDescription",
                NFTs = new List<NFT>(),
                ArtistId = 0,
                CategoryId = 0,
                IsPublic = true
            };

            var data = DatabaseMock.Instance;

            var mapper = MapperMock.Instance;
            var collectionsService = new NFTCollectionService(data, mapper);
            var artistsService = new ArtistService(data);
            var userService = new UserService(data);

            var collectionsController = new NFTCollectionsController(collectionsService, artistsService, mapper, userService, null);

            var result = collectionsController.Details(collection.Id, "InvalidInformation");

            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void CreateShouldReturnView()
        {
            var data = DatabaseMock.Instance;

            var mapper = MapperMock.Instance;
            var collectionsService = new NFTCollectionService(data, mapper);
            var artistsService = new ArtistService(data);
            var userService = new UserService(data);

            var collectionsController = new NFTCollectionsController(collectionsService, artistsService, mapper, userService, null);
            collectionsController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = ClaimsPrincipalMock.Instance()
            };

            var result = collectionsController.Create();

            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
    }
}

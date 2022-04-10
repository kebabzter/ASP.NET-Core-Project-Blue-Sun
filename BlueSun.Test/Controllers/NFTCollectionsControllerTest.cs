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
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

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

            var collectionsController = new NFTCollectionsController(collectionsService, artistsService, mapper, userService);

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
            var nfts = new List<NFT>();

            var collection = new NFTCollection
            {
                Name = "TestName",
                ImageUrl = "TestImageUrl",
                IsPublic = true,
                Description = "TestDescription",
                CategoryId = 1,
                ArtistId = 1,
                NFTs = nfts,
            };

            var data = DatabaseMock.Instance;
            data.NFTCollections.Add(collection);
            data.SaveChanges();

            var mapper = MapperMock.Instance;

            var collectionsService = new NFTCollectionService(data, mapper);

            var artistsService = new ArtistService(data);

            var userService = new UserService(data);

            var collectionsController = new NFTCollectionsController(collectionsService, artistsService, mapper, userService);

            var result = collectionsController.Details(collection.Id, collection.Name);

            Assert.Equal(collection.NFTs.Count, nfts.Count);
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
    }
}

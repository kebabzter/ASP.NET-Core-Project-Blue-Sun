namespace BlueSun.Controllers
{
    using BlueSun.Data;
    using BlueSun.Data.Models;
    using BlueSun.Infrastructure.Extensions;
    using BlueSun.Models.NFTs;
    using BlueSun.Services.NFTCollections;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class NFTsController : Controller
    {
        private readonly BlueSunDbContext data;
        private readonly INFTCollectionService collections;

        public NFTsController(BlueSunDbContext data, INFTCollectionService collections) => this.data = data;

        [Authorize]
        public IActionResult Add(int id)
        {
            var collection = this.data.NFTCollections.First(c => c.Id == id);

            return View(new AddNFTFormModel
               {
                   CollectionName = collection.Name,
                   Categories = this.GetNFTCategories()
               });
        }

        public IActionResult All()
        {
            var nfts = this.data
                .NFTs
                .OrderByDescending(n => n.Id)
                .Select(n => new NFTListingViewModel
                {
                    Id = n.Id,
                    Name = n.Name,
                    Price = n.Price,
                    ImageUrl = n.ImageUrl,
                    Category = n.Category.Name,
                    NFTCollectionName = n.NFTCollection.Name
                })
                .ToList();

            return View(nfts);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(AddNFTFormModel nft, int id)
        {

            if (!ModelState.IsValid)
            {
                nft.Categories = this.GetNFTCategories();

                return View(nft);
            }

            var userId = this.User.Id();

            var collection = this.data.NFTCollections.First(c => c.Id == id);

            var nftData = new NFT
            {
                Name = nft.Name,
                Description = nft.Description,
                Price = nft.Price,
                ImageUrl = nft.ImageUrl,
                CategoryId = collection.CategoryId,
                NFTCollectionId = id,
                OwnerId = userId
            };


            this.data.NFTs.Add(nftData);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        private IEnumerable<NFTCategoryViewModel> GetNFTCategories()
        => this.data
            .Categories
            .Select(c => new NFTCategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
            })
            .ToList();
    }
}

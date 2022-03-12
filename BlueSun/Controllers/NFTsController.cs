namespace BlueSun.Controllers
{
    using BlueSun.Data;
    using BlueSun.Data.Models;
    using BlueSun.Models.NFTs;
    using Microsoft.AspNetCore.Mvc;

    public class NFTsController : Controller
    {
        private readonly BlueSunDbContext data;

        public NFTsController(BlueSunDbContext data) => this.data = data;

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
                    Category = n.Category.Name
                })
                .ToList();

            return View(nfts);
        }

        [HttpPost]
        public IActionResult Add(AddNFTFormModel nft, int id)
        {
            if (!this.data.Categories.Any(c => c.Id == nft.CategoryId))
            {
                this.ModelState.AddModelError(nameof(nft.CategoryId), "Category does not exist!");
            }


            if (!ModelState.IsValid)
            {
                nft.Categories = this.GetNFTCategories();

                return View(nft);
            }

            var nftData = new NFT
            {
                Name = nft.Name,
                Description = nft.Description,
                Price = nft.Price,
                ImageUrl = nft.ImageUrl,
                CategoryId = nft.CategoryId,
                NFTCollectionId = id
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

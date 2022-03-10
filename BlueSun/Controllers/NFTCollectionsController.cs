namespace BlueSun.Controllers
{
    using BlueSun.Data;
    using BlueSun.Data.Models;
    using BlueSun.Models.NFTCollections;
    using BlueSun.Models.NFTs;
    using Microsoft.AspNetCore.Mvc;

    public class NFTCollectionsController : Controller
    {
        private readonly BlueSunDbContext data;

        public NFTCollectionsController(BlueSunDbContext data) => this.data = data;

        public IActionResult Create() => View(new CreateNFTCollectionFormModel
        {
            Categories = this.GetNFTCollectionCategories()
        });

        public IActionResult All()
        {
            var nftsCollections = this.data
                .NFTCollections
                .OrderByDescending(n => n.Id)
                .Select(n => new NFTCollectionListingViewModel
                {
                    Id = n.Id,
                    Name = n.Name,
                    ImageUrl = n.ImageUrl,
                    Category = n.Category.Name
                })
                .ToList();

            return View(nftsCollections);
        }

        [HttpPost]
        public IActionResult Create(CreateNFTCollectionFormModel nftCollection)
        {
            if (!this.data.Categories.Any(c => c.Id == nftCollection.CategoryId))
            {
                this.ModelState.AddModelError(nameof(nftCollection.CategoryId), "Category does not exist!");
            }

            if (this.data.NFTCollections.Any(c => c.Name == nftCollection.Name))
            {
                this.ModelState.AddModelError(nameof(nftCollection.Name), $"NFT Collection with name {nftCollection.Name} already exists.");
            }


            if (!ModelState.IsValid)
            {
                nftCollection.Categories = this.GetNFTCollectionCategories();

                return View(nftCollection);
            }

            var nftCollectionData = new NFTCollection
            {
                Name = nftCollection.Name,
                Description = nftCollection.Description,
                ImageUrl = nftCollection.ImageUrl,
                CategoryId = nftCollection.CategoryId,
            };

            this.data.NFTCollections.Add(nftCollectionData);
            this.data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Details(int id)
        {
            var collection = this.data.NFTCollections.FirstOrDefault(n => n.Id == id);

            var nftsQuery = this.data.NFTs.AsQueryable();

            if (collection != null)
            {
                nftsQuery = nftsQuery.Where(n => n.NFTCollectionId == id);
            }

            var nfts = nftsQuery
                .Select(n => new NFTListingViewModel
                {
                    Id = n.Id,
                    Name = n.Name,
                    ImageUrl = n.ImageUrl,
                    Price = n.Price,
                    Category = n.Category.Name
                })
                .ToList();

            return View(new CollectionNFTsQueryModel
            {
                Name = collection.Name,
                NFTs = nfts,
                Id = collection.Id
            });

        }

        private IEnumerable<NFTCollectionCategoryViewModel> GetNFTCollectionCategories()
        => this.data
            .Categories
            .Select(c => new NFTCollectionCategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
            })
            .ToList();
    }
}

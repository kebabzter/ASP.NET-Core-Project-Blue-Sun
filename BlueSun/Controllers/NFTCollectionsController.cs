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

            return RedirectToAction("Index","Home");
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

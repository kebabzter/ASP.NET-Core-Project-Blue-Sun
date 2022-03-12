namespace BlueSun.Controllers
{
    using BlueSun.Data;
    using BlueSun.Data.Models;
    using BlueSun.Infrastructure;
    using BlueSun.Models.NFTCollections;
    using BlueSun.Models.NFTs;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Security.Claims;

    public class NFTCollectionsController : Controller
    {
        private readonly BlueSunDbContext data;

        public NFTCollectionsController(BlueSunDbContext data) => this.data = data;

        [Authorize]
        public IActionResult Create()
        {
            if(!this.UserIsArtist())
            {
                return RedirectToAction(nameof(ArtistsController.Become), "Artists");
            }

            return View(new CreateNFTCollectionFormModel
            {
                Categories = this.GetNFTCollectionCategories()
            });
        }

        public IActionResult All([FromQuery]AllNFTCollectionsQueryModel query)
        {
            var collectionsQuery = this.data.NFTCollections.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Category))
            {
                collectionsQuery = collectionsQuery.Where(c => c.Category.Name == query.Category);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                collectionsQuery = collectionsQuery.Where(c =>
                c.Name.ToLower().Contains(query.SearchTerm.ToLower()) ||
                c.Description.ToLower().Contains(query.SearchTerm.ToLower()));
            }

            var totalCollections = collectionsQuery.Count();

            collectionsQuery = query.Sorting switch
            {
                CollectionSorting.DateCreated => collectionsQuery.OrderByDescending(c => c.Id),
                CollectionSorting.Name => collectionsQuery.OrderBy(c => c.Name),
                _ => collectionsQuery.OrderByDescending(c => c.Id)
            };

            var nftsCollections = collectionsQuery
                .Skip((query.CurrentPage - 1) * AllNFTCollectionsQueryModel.CollectionsPerPage)
                .Take(AllNFTCollectionsQueryModel.CollectionsPerPage)
                .Select(c => new NFTCollectionListingViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImageUrl = c.ImageUrl,
                    Category = c.Category.Name
                })
                .ToList();

            var categories = this.data
                .NFTCollections
                .Select(c => c.Category.Name)
                .OrderBy(c => c)
                .Distinct()
                .ToList();

            query.Collections = nftsCollections;
            query.Categories = categories;
            query.TotalCollections = totalCollections;

            return View(query);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CreateNFTCollectionFormModel nftCollection)
        {
            var artistId = this.data
                .Artists
                .Where(a => a.UserId == this.User.GetId())
                .Select(a => a.Id)
                .FirstOrDefault();

            if (artistId == 0)
            {
                return RedirectToAction(nameof(ArtistsController.Become), "Artists");
            }

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
                ArtistId = artistId
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

        private bool UserIsArtist()
            => this.data
                .Artists
                .Any(d => d.UserId == this.User.GetId());

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

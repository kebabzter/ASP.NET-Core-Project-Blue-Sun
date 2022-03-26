namespace BlueSun.Controllers
{
    using AutoMapper;
    using BlueSun.Data;
    using BlueSun.Infrastructure;
    using BlueSun.Models.NFTCollections;
    using BlueSun.Models.NFTs;
    using BlueSun.Services.Artists;
    using BlueSun.Services.NFTCollections;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    public class NFTCollectionsController : Controller
    {
        private readonly INFTCollectionService collections;
        private readonly BlueSunDbContext data;
        private readonly IArtistService artists;
        private readonly IMapper mapper;

        public NFTCollectionsController(
            INFTCollectionService collections,
            BlueSunDbContext data,
            IArtistService artists, 
            IMapper mapper)
        {
            this.data = data;
            this.collections = collections;
            this.artists = artists;
            this.mapper = mapper;
        }

        [Authorize]
        public IActionResult Create()
        {
            
            if (!this.artists.IsArtist(this.User.Id()))
            {
                return RedirectToAction(nameof(ArtistsController.Become), "Artists");
            }

            return View(new NFTCollectionFormModel
            {
                Categories = this.collections.AllCategories()
            });
        }

        public IActionResult All([FromQuery] AllNFTCollectionsQueryModel query)
        {
            var queryResult = this.collections.All(
                query.Category,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllNFTCollectionsQueryModel.CollectionsPerPage);

            var categories = this.collections.AllCategories();

            query.Collections = queryResult.Collections;
            query.Categories = categories;
            query.TotalCollections = queryResult.TotalCollections;

            return View(query);
        }

        [Authorize]
        public IActionResult MyCollections()
        {
            var myCollections = this.collections.ByUser(this.User.Id());

            return View(myCollections);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(NFTCollectionFormModel nftCollection)
        {
            var artistId = this.artists.IdByUser(this.User.Id());

            if (artistId == 0)
            {
                return RedirectToAction(nameof(ArtistsController.Become), "Artists");
            }

            if (!this.collections.CategoryExists(nftCollection.CategoryId))
            {
                this.ModelState.AddModelError(nameof(nftCollection.CategoryId), "Category does not exist!");
            }

            if (this.collections.NFTCollectionExists(nftCollection.Name))
            {
                this.ModelState.AddModelError(nameof(nftCollection.Name), $"NFT Collection with name {nftCollection.Name} already exists.");
            }


            if (!ModelState.IsValid)
            {
                nftCollection.Categories = this.collections.AllCategories();

                return View(nftCollection);
            }

            this.collections.Create(
               nftCollection.Name,
               nftCollection.Description,
               nftCollection.ImageUrl,
               nftCollection.CategoryId,
               artistId);


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

            var artist = this.data.Artists.FirstOrDefault(a => a.Id == collection.ArtistId);

            return View(new CollectionNFTsQueryModel
            {
                Name = collection.Name,
                NFTs = nfts,
                Id = collection.Id,
                ArtistUserId = artist.UserId
            });

        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = this.User.Id();

            if (!this.artists.IsArtist(userId) && !User.IsAdmin())
            {
                return RedirectToAction(nameof(ArtistsController.Become), "Artists");
            }

            var nftCollection = this.collections.Details(id);

            if (nftCollection.UserId != userId && !User.IsAdmin())
            {
                return Unauthorized();
            }

            var nftCollectionForm = this.mapper.Map<NFTCollectionFormModel>(nftCollection);

            nftCollectionForm.Categories = this.collections.AllCategories();

            return View(nftCollectionForm);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int id, NFTCollectionFormModel nftCollection)
        {
            var artistId = this.artists.IdByUser(this.User.Id());

            if (artistId == 0 && !User.IsAdmin())
            {
                return RedirectToAction(nameof(ArtistsController.Become), "Artists");
            }

            if (!this.collections.CategoryExists(nftCollection.CategoryId))
            {
                this.ModelState.AddModelError(nameof(nftCollection.CategoryId), "Category does not exist!");
            }

            //TODO: Think of a way to display the message whenever a collection with current name exists but the name is not the current name!
            //if (this.collections.NFTCollectionExists(nftCollection.Name))
            //{
            //    this.ModelState.AddModelError(nameof(nftCollection.Name), $"NFT Collection with name {nftCollection.Name} already exists.");
            //}


            if (!ModelState.IsValid)
            {
                nftCollection.Categories = this.collections.AllCategories();

                return View(nftCollection);
            }

            if (!this.collections.IsByArtist(id,artistId) && !User.IsAdmin())
            {
                return BadRequest();
            }

            this.collections.Edit(
               id,
               nftCollection.Name,
               nftCollection.Description,
               nftCollection.ImageUrl,
               nftCollection.CategoryId);

            return RedirectToAction("Index", "Home");
        }
    }
}

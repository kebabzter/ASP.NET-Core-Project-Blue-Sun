namespace BlueSun.Controllers
{
    using AutoMapper;
    using BlueSun.Infrastructure.Extensions;
    using BlueSun.Models.NFTCollections;
    using BlueSun.Services.Artists;
    using BlueSun.Services.NFTCollections;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static WebConstants;

    public class NFTCollectionsController : Controller
    {
        private readonly INFTCollectionService collections;
        private readonly IArtistService artists;
        private readonly IMapper mapper;

        public NFTCollectionsController(
            INFTCollectionService collections,
            IArtistService artists, 
            IMapper mapper)
        {
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

            var collectionId = this.collections.Create(
               nftCollection.Name,
               nftCollection.Description,
               nftCollection.ImageUrl,
               nftCollection.CategoryId,
               artistId);

            TempData[GlobalMessageKey] = "You successfully added a NFT collection and it is waiting for approval!";

            return RedirectToAction(nameof(Details), new { id = collectionId, information = nftCollection.GetInformation()});
        }

        [Authorize]
        public IActionResult Details(int id, string information)
         {
            var collection = this.collections.Details(id);

            if (information != collection.GetInformation())
            {
                return BadRequest();
            }

            var nfts = collections.GetNFTs(id);

            var artistUserId = artists.UserById(collection.ArtistId);

            return View(new CollectionNFTsQueryModel
            {
                Name = collection.Name,
                NFTs = nfts,
                Id = collection.Id,
                ArtistUserId = artistUserId,
                ImageUrl = collection.ImageUrl,
                Description = collection.Description
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

            var edited = this.collections.Edit(
               id,
               nftCollection.Name,
               nftCollection.Description,
               nftCollection.ImageUrl,
               nftCollection.CategoryId,
               this.User.IsAdmin());

            if (!edited)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = $"You successfully edited your NFT collection{(this.User.IsAdmin() ? string.Empty : " and it is waiting for approval")}!";

            return RedirectToAction(nameof(Details), new { id , information = nftCollection.GetInformation() });
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var artistId = this.artists.IdByUser(this.User.Id());

            if (!collections.IsByArtist(id,artistId) && !User.IsAdmin())
            {
                return Unauthorized();
            }

            collections.Delete(id);

            TempData[GlobalMessageKey] = "You successfully deleted your NFT collection!";

            return RedirectToAction(nameof(MyCollections));
        }
    }
}

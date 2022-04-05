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
                .Where(n => n.NFTCollection.IsPublic == true)
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

        [Authorize]
        public IActionResult Details(int id)
        {
            var nft = this.data.NFTs.First(n => n.Id == id);

            var nftCollection = this.data.NFTCollections.First(c => c.Id == nft.NFTCollectionId);

            var artist = this.data.Artists.First(a => a.Id == nftCollection.ArtistId);

            var user = this.data.Users.First(u => u.Id == this.User.Id());

            var owner = this.data.Users.First(u => u.Id == nft.OwnerId);

            var nftData = new NFTDetailsViewModel
            {
                Id = nft.Id,
                Name = nft.Name,
                Price = nft.Price,
                Description = nft.Description,
                OwnerId = nft.Owner.Id,
                OwnerName = owner.FullName,
                UserHasWallet = user.HasWallet,
                UserIsOwner = user.Id == owner.Id,
                ArtistName = artist.Name,
                ArtistId = artist.Id,
                ArtistUserId = artist.UserId,
                ImageUrl = nft.ImageUrl,
                NFTCollectionName = nftCollection.Name,
                NFTCollectionId = nftCollection.Id
            };

            return View(nftData);
        }

        [Authorize]
        public IActionResult ConnectWallet(int id)
        {
            var user = this.data.Users.First(u => u.Id == this.User.Id());

            if (user.HasWallet)
            {
                return BadRequest();
            }

            user.HasWallet = true;
            user.Wallet = new Wallet
            {
                Balance = 10000
            };

            this.data.SaveChanges();

            return RedirectToAction("Details", "NFTs", new { id });
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

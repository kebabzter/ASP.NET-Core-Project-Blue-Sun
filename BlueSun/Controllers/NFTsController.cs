namespace BlueSun.Controllers
{
    using BlueSun.Data;
    using BlueSun.Data.Models;
    using BlueSun.Infrastructure.Extensions;
    using BlueSun.Models.NFTs;
    using BlueSun.Services.NFTs;
    using BlueSun.Services.NFTs.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class NFTsController : Controller
    {
        private readonly BlueSunDbContext data;
        private readonly INFTsService nfts;

        public NFTsController(BlueSunDbContext data, INFTsService nfts)
        {
            this.data = data;
            this.nfts = nfts;
        }

        [Authorize]
        public IActionResult Add(int id)
        {
            string collectionName = nfts.GetCollectionName(id);

            return View(new AddNFTFormModel
               {
                   CollectionName = collectionName,
                   Categories = this.GetNFTCategories()
               });
        }

        public IActionResult All()
        {
            var nftsData = nfts.All();

            return View(new AllNFTsModel { NFTs = nftsData });
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

            nfts.Add(nft, id, userId);

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var nft = this.data.NFTs.First(n => n.Id == id);

            var nftCollection = this.data.NFTCollections.First(c => c.Id == nft.NFTCollectionId);

            var artist = this.data.Artists.First(a => a.Id == nftCollection.ArtistId);

            var user = this.data.Users.First(u => u.Id == this.User.Id());

            var userWallet = this.data.Wallets.FirstOrDefault(w => w.UserId == this.User.Id());

            if (userWallet == null)
            {
                userWallet = new Wallet { Balance = 0 };
            }

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
                UserBalanceAfterPurchase = userWallet.Balance - nft.Price,
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

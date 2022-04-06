namespace BlueSun.Controllers
{
    using BlueSun.Data;
    using BlueSun.Data.Models;
    using BlueSun.Infrastructure.Extensions;
    using BlueSun.Models.NFTs;
    using BlueSun.Models.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static WebConstants;

    [Authorize]
    public class UsersController : Controller
    {
        private readonly BlueSunDbContext data;

        public UsersController(BlueSunDbContext data)
        {
            this.data = data;
        }

        [Authorize]
        public IActionResult PersonalCollection(string id)
        {
            var user = this.data.Users.First(u => u.Id == id);

            var nftsData = this.data
               .NFTs
               .Where(n => n.OwnerId == id)
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

            var userData = new UsersPersonalCollectionViewModel
            {
                UserName = user.FullName,
                nfts = nftsData,
            };

            return View(userData);
        }

        [Authorize]
        public IActionResult FillWallet()
        {
            var wallet = this.data.Wallets.First(u => u.UserId == this.User.Id());

            wallet.Balance += 10000;

            this.data.SaveChanges();

            TempData[GlobalMessageKey] = $"You successfully added funds to your wallet!";
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Purchase(int id)
        {
            var user = this.data.Users.First(u => u.Id == this.User.Id());
            var wallet = this.data.Wallets.First(u => u.UserId == this.User.Id());
            var nft = this.data.NFTs.First(n => n.Id == id);
            var owner = this.data.Users.First(u => u.Id == nft.OwnerId);
            var ownerWallet = this.data.Wallets.First(w => w.UserId == owner.Id);

            if (nft.Price > wallet.Balance)
            {
                return RedirectToAction("Details", "NFTs", new { id });
            }

            nft.OwnerId = this.User.Id();
            ownerWallet.Balance += nft.Price;
            nft.Owner = user;
            user.Wallet.Balance -= nft.Price;

            this.data.SaveChanges();

            TempData[GlobalMessageKey] = $"You successfully purchased {nft.Name}!";
            return RedirectToAction("Details", "NFTs", new { id });
        }

        //TODO: Finish wallet system if possible!

        [Authorize]
        public IActionResult ConnectWallet()
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

            return RedirectToAction("Index", "Home");
        }
    }
}

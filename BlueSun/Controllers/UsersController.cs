namespace BlueSun.Controllers
{
    using BlueSun.Data;
    using BlueSun.Data.Models;
    using BlueSun.Infrastructure.Extensions;
    using BlueSun.Models.NFTs;
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

        public IActionResult PersonalCollection()
        {
            var nfts = this.data
               .NFTs
               .Where(n => n.OwnerId == this.User.Id())
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

        public IActionResult Purchase(int id)
        {
            var user = this.data.Users.First(u => u.Id == this.User.Id());
            var wallet = this.data.Wallets.First(u => u.UserId == this.User.Id());
            var nft = this.data.NFTs.First(n => n.Id == id);

            if (nft.Price > wallet.Balance)
            {
                TempData[GlobalMessageKey] = "Insufficient funds!";
                return RedirectToAction("Details", "NFTs", new { id });
            }

            nft.OwnerId = this.User.Id();
            nft.Owner = user;
            user.Wallet.Balance -= nft.Price;

            this.data.SaveChanges();

            TempData[GlobalMessageKey] = $"You successfully purchased {nft.Name}!";
            return RedirectToAction("Details", "NFTs", new { id });
        }

        //TODO: Finish wallet system if possible!

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
    }
}

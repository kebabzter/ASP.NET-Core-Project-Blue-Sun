namespace BlueSun.Controllers
{
    using BlueSun.Data;
    using BlueSun.Infrastructure.Extensions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class UsersController : Controller
    {
        private readonly BlueSunDbContext data;

        public UsersController(BlueSunDbContext data)
        {
            this.data = data;
        }

        public IActionResult Purchase(int id)
        {
            var user = this.data.Users.First(u => u.Id == this.User.Id());
            var nft = this.data.NFTs.First(n => n.Id == id);

            if (nft.Price > user.Balance)
            {
                return RedirectToAction("Details", "NFTs", id);
            }

            nft.OwnerId = user.Id;
            user.Balance -= nft.Price;

            this.data.SaveChanges();

            return RedirectToAction("Details", "NFTs", id);
        }

        //TODO: Finish wallet system if possible!
        //[HttpGet]
        //public IActionResult ConnectWallet()
        //{
        //    return View();
        //}

        //public IActionResult ConnectWallet()
        //{
        //    var user = this.data.Users.First(u => u.Id == this.User.Id());

        //    if (user.HasWallet)
        //    {
        //        return BadRequest();
        //    }

        //    user.HasWallet = true;

        //    return RedirectToAction("/");
        //}
    }
}

namespace BlueSun.Controllers
{
    using BlueSun.Infrastructure.Extensions;
    using BlueSun.Services.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static WebConstants;

    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserService users;

        public UsersController(IUserService users)
        {
            this.users = users;
        }

        [Authorize]
        public IActionResult PersonalCollection(string id)
        {
            var nftsData = users.GetPersonalCollection(id);

            return View(nftsData);
        }

        [Authorize]
        public IActionResult FillWallet()
        {
            var userId = this.User.Id();

            users.FillWallet(userId);

            TempData[GlobalMessageKey] = $"You successfully added funds to your wallet!";
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Purchase(int id)
        {
            var userId = this.User.Id();
            var successfulPurchase = users.Purchase(id, userId);

            if (!successfulPurchase)
            {
                return BadRequest();
            }

            TempData[GlobalMessageKey] = $"You successfully purchased this item!";
            return RedirectToAction("Details", "NFTs", new { id });
        }

        [Authorize]
        public IActionResult ConnectWallet()
        {
            var userId = this.User.Id();

            var canConnect = users.ConnectWallet(userId);

            if (!canConnect)
            {
                return BadRequest();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}

namespace BlueSun.Controllers
{
    using AspNetCoreHero.ToastNotification.Abstractions;
    using BlueSun.Infrastructure.Extensions;
    using BlueSun.Services.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static WebConstants;

    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserService users;
        private readonly INotyfService notyf;

        public UsersController(
            IUserService users,
            INotyfService notyf)
        {
            this.users = users;
            this.notyf = notyf;
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

            notyf.Success("Successfully filled wallet!");

            return RedirectToAction(nameof(HomeController.Index), "Home");
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

             notyf.Success("You successfully purchased an item!");
            return RedirectToAction(nameof(NFTsController.Details), "NFTs", new { id });
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

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}

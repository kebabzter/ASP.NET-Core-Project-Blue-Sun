namespace BlueSun.Controllers
{
    using AspNetCoreHero.ToastNotification.Abstractions;
    using BlueSun.Infrastructure.Extensions;
    using BlueSun.Models.Artists;
    using BlueSun.Services.Artists;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static WebConstants;
    public class ArtistsController : Controller
    {
        private readonly IArtistService artists;
        private readonly INotyfService notyf;

        public ArtistsController(IArtistService artists, INotyfService notyf)
        {
            this.artists = artists;
            this.notyf = notyf;
        }

        [Authorize]
        public IActionResult Become() => View();

        [HttpPost]
        [Authorize]
        public IActionResult Become(BecomeArtistFormModel artist)
        {
            var userId = this.User.Id();

            var userIdAlreadyArtist = artists.IsArtist(userId);

            if (userIdAlreadyArtist)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(artist);
            }

            artists.AddArtist(artist.PhoneNumber, userId);

            notyf.Success("Thank you for becoming an artist!");

            return RedirectToAction(nameof(NFTCollectionsController.All), "NFTCollections");
        }
    }
}

namespace BlueSun.Controllers
{
    using BlueSun.Data;
    using BlueSun.Data.Models;
    using BlueSun.Infrastructure.Extensions;
    using BlueSun.Models.Artists;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static WebConstants;
    public class ArtistsController : Controller
    {
        private readonly BlueSunDbContext data;

        public ArtistsController(BlueSunDbContext data)
        {
            this.data = data;
        }

        [Authorize]
        public IActionResult Become() => View();

        [HttpPost]
        [Authorize]
        public IActionResult Become(BecomeArtistFormModel artist)
        {
            var userId = this.User.Id();

            var userIdAlreadyArtist = this.data
                .Artists
                .Any(a => a.UserId == userId);

            if (userIdAlreadyArtist)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(artist);
            }

            var artistData = new Artist
            {
                Name = artist.Name,
                PhoneNumber = artist.PhoneNumber,
                UserId = userId
            };

            this.data.Artists.Add(artistData);
            this.data.SaveChanges();

            this.TempData[GlobalMessageKey] = "Thank you for becoming an artist!";

            return RedirectToAction("All", "NFTCollections");
        }
    }
}

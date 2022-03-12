﻿namespace BlueSun.Controllers
{
    using BlueSun.Data;
    using BlueSun.Data.Models;
    using BlueSun.Infrastructure;
    using BlueSun.Models.Artists;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

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
            var userId = this.User.GetId();

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

            return RedirectToAction("All", "NFTCollections");
        }
    }
}

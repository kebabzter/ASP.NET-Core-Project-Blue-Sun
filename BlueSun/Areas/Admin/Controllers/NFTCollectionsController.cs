namespace BlueSun.Areas.Admin.Controllers
{
    using BlueSun.Data;
    using BlueSun.Infrastructure.Extensions;
    using BlueSun.Services.Artists;
    using BlueSun.Services.NFTCollections;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class NFTCollectionsController : AdminController
    {
        private readonly INFTCollectionService collections;
        private readonly BlueSunDbContext data;
        private readonly IArtistService artists;

        public NFTCollectionsController(
            INFTCollectionService collections,
            BlueSunDbContext data,
            IArtistService artists)
        {
            this.data = data;
            this.collections = collections;
            this.artists = artists;
        }

        public IActionResult All()
        {
            var collections = this.collections
                .All(publicOnly: false)
                .Collections;

            return View(collections);
        }

        public IActionResult ChangeVisibility(int id)
        {
            this.collections.ChangeVisibility(id);

            return RedirectToAction(nameof(All));
        }

        public IActionResult Delete(int id)
        {
            var artistId = this.artists.IdByUser(this.User.Id());

            var collection = this.data.NFTCollections.First(c => c.Id == id);

            if (!User.IsAdmin())
            {
                return Unauthorized();
            }

            var nftsToRemove = this.data.NFTs.Where(n => n.NFTCollectionId == collection.Id);

            foreach (var nft in nftsToRemove)
            {
                this.data.NFTs.Remove(nft);
            }
            this.data.NFTCollections.Remove(collection);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }
    }
}

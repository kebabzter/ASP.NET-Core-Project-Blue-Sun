namespace BlueSun.Areas.Admin.Controllers
{
    using BlueSun.Services.NFTCollections;
    using Microsoft.AspNetCore.Mvc;

    public class NFTCollectionsController : AdminController
    {
        private readonly INFTCollectionService collections;

        public NFTCollectionsController(INFTCollectionService collections) => this.collections = collections;

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
    }
}

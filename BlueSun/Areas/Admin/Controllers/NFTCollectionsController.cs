namespace BlueSun.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class NFTCollectionsController : AdminController
    {
        public IActionResult Index() => View();
    }
}

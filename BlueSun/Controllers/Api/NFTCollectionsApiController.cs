namespace BlueSun.Controllers.Api
{
    using BlueSun.Data;
    using BlueSun.Models.Api.NFTCollections;
    using BlueSun.Models.NFTCollections;
    using BlueSun.Services.NFTCollections;
    using Microsoft.AspNetCore.Mvc;
    
    [ApiController]
    [Route("/api/collections")]
    public class NFTCollectionsApiController : ControllerBase
    {
        private readonly INFTCollectionService collections;

        public NFTCollectionsApiController(INFTCollectionService collections) 
            => this.collections = collections;

        [HttpGet]
        public NFTCollectionQueryServiceModel All([FromQuery] AllNFTCollectionsApiRequestModel query) 
            => this.collections.All(
                query.Category,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                query.CollectionsPerPage);
    }
}

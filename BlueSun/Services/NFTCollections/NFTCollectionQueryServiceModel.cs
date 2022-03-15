namespace BlueSun.Services.NFTCollections
{
    using BlueSun.Models.Api.NFTCollections;

    public class NFTCollectionQueryServiceModel
    {
        public int CurrentPage { get; init; }

        public int CollectionsPerPage { get; init; }

        public int TotalCollections { get; set; }

        public IEnumerable<NFTCollectionServiceModel> Collections { get; init; }
    }
}

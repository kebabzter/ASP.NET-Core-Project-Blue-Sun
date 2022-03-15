namespace BlueSun.Models.Api.NFTCollections
{
    using BlueSun.Models.NFTCollections;

    public class AllNFTCollectionsApiRequestModel
    {
        public string Category { get; init; }

        public string SearchTerm { get; init; }

        public CollectionSorting Sorting { get; init; }

        public int CurrentPage { get; init; } = 1;

        public int CollectionsPerPage { get; init; } = 10;

        public int TotalCollections { get; init; }
    }
}

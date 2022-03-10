namespace BlueSun.Models.NFTCollections
{
    public class AllNFTCollectionsQueryModel
    {
        public IEnumerable<string> Categories { get; init; }

        public IEnumerable<NFTCollectionListingViewModel> Collections { get; init; }
    }
}

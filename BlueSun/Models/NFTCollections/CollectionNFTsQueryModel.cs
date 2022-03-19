namespace BlueSun.Models.NFTCollections
{
    using BlueSun.Models.NFTs;

    public class CollectionNFTsQueryModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public IEnumerable<NFTListingViewModel> NFTs { get; init; }

        public string ArtistUserId { get; init; }
    }
}

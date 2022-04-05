namespace BlueSun.Models.NFTCollections
{
    using BlueSun.Services.NFTs.Models;

    public class CollectionNFTsQueryModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public IEnumerable<NFTListingServiceModel> NFTs { get; init; }

        public string ArtistUserId { get; init; }

        public string ImageUrl { get; init; }
    }
}

namespace BlueSun.Models.Home
{
    using BlueSun.Services.NFTCollections.Models;

    public class IndexViewModel
    {
        public int TotalNFTCollections { get; init; }

        public int TotalUsers { get; init; }

        public int TotalNFTs { get; init; }

        public IList<LatestNFTCollectionServiceModel> NFTCollections { get; init; }
    }
}

namespace BlueSun.Models.Home
{
    public class IndexViewModel
    {
        public int TotalNFTCollections { get; init; }

        public int TotalUsers { get; init; }

        public int TotalNFTs { get; init; }

        public List<NFTCollectionIndexViewModel> NFTCollections { get; init; }
    }
}

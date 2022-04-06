namespace BlueSun.Models.NFTs
{

    public class NFTListingViewModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public decimal Price { get; init; }

        public string ImageUrl { get; init; }

        public string Category { get; init; }

        public string NFTCollectionName { get; init; }
    }
}

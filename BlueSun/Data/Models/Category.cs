namespace BlueSun.Data.Models
{
    public class Category
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public ICollection<NFTCollection> NFTCollections { get; set; } = new List<NFTCollection>();

        public ICollection<NFT> NFTs { get; set; } = new List<NFT>();

    }
}

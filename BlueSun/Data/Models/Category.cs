namespace BlueSun.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Category;

    public class Category
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; init; }

        public ICollection<NFTCollection> NFTCollections { get; set; } = new List<NFTCollection>();

        public ICollection<NFT> NFTs { get; set; } = new List<NFT>();

    }
}

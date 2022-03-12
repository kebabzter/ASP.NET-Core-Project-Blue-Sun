namespace BlueSun.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static DataConstants.NFTCollection;
    public class NFTCollection
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; init; }

        [Required]
        [Url]
        public string ImageUrl { get; set; }

        public ICollection<NFT> NFTs { get; init; } = new List<NFT>();

        public int ArtistId { get; init; }

        public Artist Artist { get; init; }
    }
}

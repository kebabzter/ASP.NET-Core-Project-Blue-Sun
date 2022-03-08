namespace BlueSun.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class NFTCollection
    {
        public int Id { get; init; }

        public string Description { get; set; }

        public string Name { get; set; }

        public ICollection<NFT> NFTs { get; init; } = new List<NFT>();

        [Required]
        [ForeignKey(nameof(Artist))]
        public int ArtistId { get; set; }

        public Artist Artist { get; init; }
    }
}

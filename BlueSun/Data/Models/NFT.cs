namespace BlueSun.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static Data.DataConstants.NFT;
    public class NFT
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Range(MinPrice, MaxPrice)]
        [Column(TypeName = "decimal(5,2)")]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; init; }

        [Required]
        [ForeignKey(nameof(NFTCollection))]
        public int NFTCollectionId { get; set; }

        [Required]
        public NFTCollection NFTCollection { get; init; }

        [Required]
        public string ImageUrl { get; set; }
    }
}

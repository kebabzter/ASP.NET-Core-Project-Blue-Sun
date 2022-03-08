namespace BlueSun.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static Data.DataConstants;
    public class NFT
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(NFTNameMaxLength)]
        public string Name { get; set; }

        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; }

        //[Required]
        //[ForeignKey(nameof(NFTCollection))]
        //public int NFTCollectionId { get; set; }

        //[Required]
        //public NFTCollection NFTCollection { get; init; }

        public int CategoryId { get; set; }

        public Category Category { get; init; }

        [Required]
        public string ImageUrl { get; set; }
    }
}

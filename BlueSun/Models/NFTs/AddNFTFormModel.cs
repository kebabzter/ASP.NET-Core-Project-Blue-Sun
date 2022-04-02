namespace BlueSun.Models.NFTs
{
    using BlueSun.Data.Models;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.NFT;
    public class AddNFTFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; init; }

        [Range(
            MinPrice,
            MaxPrice,
            ErrorMessage = "Price must be between 0.00 and 10000.00.")]
        public decimal Price { get; init; }

        [Required]
        [StringLength(DescriptionMaxLength,
            MinimumLength = DescriptionMinLength,
            ErrorMessage = "The field Description must be a string with a minimum length of {2}.")]
        public string Description { get; init; }

        [Required]
        [Display(Name = "Image URL")]
        [Url]
        public string ImageUrl { get; init; }

        //[Display(Name = "Category")]
        //public int CategoryId { get; init; }

        public string CollectionName { get; set; }

        public NFTCollection NFTCollection { get; init; }

        public IEnumerable<NFTCategoryViewModel> Categories { get; set; }
    }
}

namespace BlueSun.Models.NFTs
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;
    public class AddNFTFormModel
    {
        [Required]
        [StringLength(NFTNameMaxLength, MinimumLength = NFTNameMinLength)]
        public string Name { get; init; }

        [Range(
            NFTMinPrice,
            NFTMaxPrice,
            ErrorMessage = "Price must be between 0.00 and 10000.00.")]
        public decimal Price { get; init; }

        [Required]
        [StringLength(int.MaxValue,
            MinimumLength = NFTDescriptionMinLength,
            ErrorMessage = "The field Description must be a string with a minimum length of {2}.")]
        public string Description { get; init; }

        [Required]
        [Display(Name = "Image URL")]
        [Url]
        public string ImageUrl { get; init; }

        [Display(Name = "Category")]
        public int CategoryId { get; init; }

        public IEnumerable<NFTCategoryViewModel> Categories { get; set; }
    }
}

namespace BlueSun.Models.NFTCollections
{
    using BlueSun.Data.Models;
    using System.ComponentModel.DataAnnotations;
    using static Data.DataConstants;

    public class CreateNFTCollectionFormModel
    {
        [Required]
        [StringLength(NFTCollectionNameMaxLength, MinimumLength = NFTCollectionNameMinLength)]
        public string Name { get; init; }

        [Required]
        [StringLength(NFTCollectionDescriptionMaxLength,
            MinimumLength = NFTCollectionDescriptionMinLength,
            ErrorMessage = "The field Description must be a string with a minimum length of {2}.")]
        public string Description { get; init; }

        [Display(Name = "Category")]
        public int CategoryId { get; init; }

        [Required]
        [Display(Name = "Image URL")]
        [Url]
        public string ImageUrl { get; init; }

        public IEnumerable<NFTCollectionCategoryViewModel> Categories { get; set; }
    }
}

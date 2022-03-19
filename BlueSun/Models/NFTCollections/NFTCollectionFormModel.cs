namespace BlueSun.Models.NFTCollections
{
    using BlueSun.Data.Models;
    using BlueSun.Services.NFTCollections;
    using System.ComponentModel.DataAnnotations;
    using static Data.DataConstants.NFTCollection;

    public class NFTCollectionFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; init; }

        [Required]
        [StringLength(DescriptionMaxLength,
            MinimumLength = DescriptionMinLength,
            ErrorMessage = "The field Description must be a string with a minimum length of {2}.")]
        public string Description { get; init; }

        [Display(Name = "Category")]
        public int CategoryId { get; init; }

        [Required]
        [Display(Name = "Image URL")]
        [Url]
        public string ImageUrl { get; init; }

        public IEnumerable<NFTCollectionCategoryServiceModel> Categories { get; set; }
    }
}

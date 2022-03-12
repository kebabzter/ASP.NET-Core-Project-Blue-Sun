namespace BlueSun.Models.Artists
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Artist;

    public class BecomeArtistFormModel
    {
        [Required]
        [StringLength(NameMaxLength,MinimumLength = NameMinLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
        [Display(Name = "Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}

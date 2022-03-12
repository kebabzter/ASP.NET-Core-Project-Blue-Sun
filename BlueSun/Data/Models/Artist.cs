namespace BlueSun.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;
    using static Data.DataConstants.Artist;

    public class Artist
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; }

        [Required]
        public string UserId { get; set; }

        public IEnumerable<NFTCollection> NFTCollections { get; init; } = new List<NFTCollection>();
    }
}

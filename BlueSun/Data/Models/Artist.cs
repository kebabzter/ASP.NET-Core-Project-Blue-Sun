namespace BlueSun.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Artist
    {
        public int Id { get; init; }

        public string Username { get; set; }

        public ICollection<NFTCollection> NFTCollections { get; set; } = new List<NFTCollection>();

        [Required]
        public string UserId { get; set; }
    }
}

namespace BlueSun.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static DataConstants.User;

    public class User : IdentityUser
    {
        [Required]
        [MaxLength(FullNameMaxLength)]
        public string FullName { get; set; }

        [Range(0, int.MaxValue)]
        [Column(TypeName = "decimal(5,2)")]
        public decimal Balance { get; set; } = 100000;

        public ICollection<NFT> NFTs { get; init; } = new List<NFT>();

        public bool HasWallet { get; set; }
    }
}

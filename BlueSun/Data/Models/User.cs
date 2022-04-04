namespace BlueSun.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;
    using static DataConstants.User;

    public class User : IdentityUser
    {
        [Required]
        [MaxLength(FullNameMaxLength)]
        public string FullName { get; set; }

        public ICollection<NFT> NFTs { get; init; } = new List<NFT>();

        public Wallet Wallet { get; set; }

        public bool HasWallet { get; set; }
    }
}

﻿namespace BlueSun.Data.Models
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

        [Column(TypeName = "decimal(7,2)")]
        public decimal Balance { get; set; }

        public ICollection<NFT> NFTs { get; init; } = new List<NFT>();
    }
}

namespace BlueSun.Services.Users.Models
{
    using BlueSun.Data.Models;

    public class UserServiceModel
    {
        public string FullName { get; set; }

        public ICollection<NFT> NFTs { get; init; } = new List<NFT>();

        public Wallet Wallet { get; set; }

        public bool HasWallet { get; set; }
    }
}

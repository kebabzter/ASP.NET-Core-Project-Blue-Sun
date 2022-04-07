namespace BlueSun.Services.Users.Models
{
    using BlueSun.Services.NFTs.Models;

    public class UsersPersonalCollectionServiceModel
    {
        public string UserName { get; set; }

        public IEnumerable<NFTListingServiceModel> NFTs { get; set; }
    }
}

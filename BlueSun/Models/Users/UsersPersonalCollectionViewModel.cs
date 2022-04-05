namespace BlueSun.Models.Users
{
    using BlueSun.Models.NFTs;

    public class UsersPersonalCollectionViewModel
    {
        public string UserName { get; set; }

        public IEnumerable<NFTListingViewModel> nfts { get; set; }
    }
}

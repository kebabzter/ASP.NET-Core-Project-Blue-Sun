namespace BlueSun.Services.Users
{
    using BlueSun.Services.Users.Models;

    public interface IUserService
    {
        public decimal GetBalanceByUserId(string userId);

        public UsersPersonalCollectionServiceModel GetPersonalCollection(string id);

        public void FillWallet(string userId);

        public bool Purchase(int nftId, string userId);

        public bool ConnectWallet(string userId);
    }
}

namespace BlueSun.Services.Users
{
    using AutoMapper;
    using BlueSun.Data;
    using BlueSun.Data.Models;
    using BlueSun.Services.NFTs.Models;
    using BlueSun.Services.Users.Models;

    public class UserService : IUserService
    {
        private readonly BlueSunDbContext data;

        public UserService(BlueSunDbContext data)
        {
            this.data = data;
        }

        public void FillWallet(string userId)
        {
            var wallet = this.data.Wallets.First(u => u.UserId == userId);

            wallet.Balance += 3000;

            this.data.SaveChanges();
        }

        public decimal GetBalanceByUserId(string userId)
        {
            var wallet = this.data
                       .Wallets
                       .FirstOrDefault(w => w.UserId == userId);

            if (wallet == null)
            {
                return -1;
            }

            return wallet.Balance;
        }

        public UsersPersonalCollectionServiceModel GetPersonalCollection(string id)
        {
            var user = this.data.Users.First(u => u.Id == id);

            var nftsData = this.data
               .NFTs
               .Where(n => n.OwnerId == id)
               .OrderByDescending(n => n.Id)
               .Where(n => n.NFTCollection.IsPublic == true)
               .Select(n => new NFTListingServiceModel
               {
                   Id = n.Id,
                   Name = n.Name,
                   Price = n.Price,
                   ImageUrl = n.ImageUrl,
                   Category = n.Category.Name,
                   NFTCollectionName = n.NFTCollection.Name
               })
               .ToList();

            return new UsersPersonalCollectionServiceModel
            {
                UserName = user.FullName,
                NFTs = nftsData,
            };
        }

        public bool Purchase(int nftId, string userId)
        {
            var user = this.data.Users.First(u => u.Id == userId);
            var wallet = this.data.Wallets.First(u => u.UserId == userId);
            var nft = this.data.NFTs.First(n => n.Id == nftId);
            var owner = this.data.Users.First(u => u.Id == nft.OwnerId);
            var ownerWallet = this.data.Wallets.First(w => w.UserId == owner.Id);

            if (nft.Price > wallet.Balance)
            {
                return false;
            }

            nft.OwnerId = userId;
            nft.IsForSale = false;
            ownerWallet.Balance += nft.Price;
            nft.Owner = user;
            user.Wallet.Balance -= nft.Price;

            this.data.SaveChanges();

            return true;
        }
        public bool ConnectWallet(string userId)
        {
            var user = this.data.Users.First(u => u.Id == userId);

            if (user.HasWallet)
            {
                return false;
            }

            user.HasWallet = true;
            user.Wallet = new Wallet
            {
                Balance = 10000
            };

            this.data.SaveChanges();

            return true;
        }

        public bool HasWallet(string userId)
        => this.data
            .Users
            .Where(u => u.HasWallet == true)
            .Any(u => u.Id == userId);
            
    }
}

namespace BlueSun.Services.NFTs
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using BlueSun.Data;
    using BlueSun.Data.Models;
    using BlueSun.Models.NFTs;
    using BlueSun.Services.NFTs.Models;

    public class NFTsService : INFTsService
    {
        private readonly BlueSunDbContext data;

        public NFTsService(BlueSunDbContext data)
        {
            this.data = data;
        }

        public void Add(AddNFTFormModel nft, int id, string ownerId)
        {
            var collection = this.data.NFTCollections.First(c => c.Id == id);

            var nftData = new NFT
            {
                Name = nft.Name,
                Description = nft.Description,
                Price = nft.Price,
                ImageUrl = nft.ImageUrl,
                CategoryId = collection.CategoryId,
                NFTCollectionId = id,
                OwnerId = ownerId
            };


            this.data.NFTs.Add(nftData);
            this.data.SaveChanges();
        }

        public IEnumerable<NFTListingServiceModel> All() 
            => this.data
                .NFTs
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

        public string GetCollectionName(int id)
        => this.data
            .NFTCollections
            .Where(c => c.Id == id)
            .Select(c => c.Name)
            .FirstOrDefault();

        public void ForSale(int id, decimal price)
        {
            var nft = this.data.NFTs.FirstOrDefault(n => n.Id == id);

            nft.IsForSale = true;
            nft.Price = price;

            this.data.SaveChanges();
        }

        public void TakeFromMarket(int id)
        {
            var nft = this.data.NFTs.FirstOrDefault(n => n.Id == id);

            nft.IsForSale = false;

            this.data.SaveChanges();
        }

        public NFTDetailsServiceModel Details(int id, string userId)
        {
            var nft = this.data.NFTs.First(n => n.Id == id);
            var nftCollection = this.data.NFTCollections.First(c => c.Id == nft.NFTCollectionId);
            var artist = this.data.Artists.First(a => a.Id == nftCollection.ArtistId);
            var user = this.data.Users.First(u => u.Id == userId);
            var userWallet = this.data.Wallets.FirstOrDefault(w => w.UserId == userId);
            var owner = this.data.Users.First(u => u.Id == nft.OwnerId);

            if (userWallet == null)
            {
                userWallet = new Wallet { Balance = 0 };
            }

            return new NFTDetailsServiceModel
            {
                Id = nft.Id,
                Name = nft.Name,
                Price = nft.Price,
                Description = nft.Description,
                IsForSale = nft.IsForSale,
                OwnerId = nft.Owner.Id,
                OwnerName = owner.FullName,
                UserHasWallet = user.HasWallet,
                UserIsOwner = user.Id == owner.Id,
                UserBalanceAfterPurchase = userWallet.Balance - nft.Price,
                ArtistName = artist.Name,
                ArtistId = artist.Id,
                ArtistUserId = artist.UserId,
                ImageUrl = nft.ImageUrl,
                NFTCollectionName = nftCollection.Name,
                NFTCollectionId = nftCollection.Id
            };
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
    }
}

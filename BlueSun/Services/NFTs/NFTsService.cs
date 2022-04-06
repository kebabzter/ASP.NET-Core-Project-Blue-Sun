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
    }
}

namespace BlueSun.Services.NFTs
{
    using BlueSun.Models.NFTs;
    using BlueSun.Services.NFTs.Models;

    public interface INFTsService
    {
        public IEnumerable<NFTListingServiceModel> All();

        public string GetCollectionName(int id);

        public void Add(AddNFTFormModel nft, int id, string ownerId);
    }
}

namespace BlueSun.Services.NFTCollections
{
    using BlueSun.Models.NFTCollections;
    using BlueSun.Services.NFTCollections.Models;
    using BlueSun.Services.NFTs.Models;

    public interface INFTCollectionService
    {

        NFTCollectionQueryServiceModel All(
            string category = null,
            string searchTerm = null,
            CollectionSorting sorting = CollectionSorting.DateCreated,
            int currentPage = 1,
            int collectionsPerPage = int.MaxValue,
            bool publicOnly = true);

        IEnumerable<LatestNFTCollectionServiceModel> Latest();

        IEnumerable<NFTListingServiceModel> GetNFTs(int collectionId);

        int Create(
            string name,
            string description,
            string imageUrl,
            int categoryId,
            int artistId);

        bool Edit(
            int id,
            string name,
            string description,
            string imageUrl,
            int categoryId,
            bool isPublic);

        void Delete(int id);

        NFTCollectionDetailsServiceModel Details(int collectionId);

        IEnumerable<NFTCollectionServiceModel> ByUser(string userId);

        bool IsByArtist(int collectionId, int artistId);

        void ChangeVisibility(int id);

        IEnumerable<NFTCollectionCategoryServiceModel> AllCategories();

        bool CategoryExists(int categoryId);

        bool NFTCollectionExists(string collectionName);
    }
}

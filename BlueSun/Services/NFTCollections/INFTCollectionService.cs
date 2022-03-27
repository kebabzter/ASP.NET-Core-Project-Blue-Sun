namespace BlueSun.Services.NFTCollections
{
    using BlueSun.Models.NFTCollections;
    using BlueSun.Services.NFTCollections.Models;

    public interface INFTCollectionService
    {

        NFTCollectionQueryServiceModel All(
            string category,
            string searchTerm,
            CollectionSorting sorting,
            int currentPage,
            int collectionsPerPage);

        IEnumerable<LatestNFTCollectionServiceModel> Latest();

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
            int categoryId);

        NFTCollectionDetailsServiceModel Details(int collectionId);

        IEnumerable<NFTCollectionServiceModel> ByUser(string userId);

        bool IsByArtist(int collectionId, int artistId);

        IEnumerable<NFTCollectionCategoryServiceModel> AllCategories();

        bool CategoryExists(int categoryId);

        bool NFTCollectionExists(string collectionName);
    }
}

namespace BlueSun.Services.NFTCollections
{
    using BlueSun.Models.NFTCollections;

    public interface INFTCollectionService
    {
        NFTCollectionQueryServiceModel All(
            string category,
            string searchTerm,
            CollectionSorting sorting,
            int currentPage,
            int collectionsPerPage);

        IEnumerable<string> AllCollectionCategories();
    }
}

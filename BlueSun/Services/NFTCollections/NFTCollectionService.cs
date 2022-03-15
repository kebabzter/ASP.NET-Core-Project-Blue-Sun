namespace BlueSun.Services.NFTCollections
{
    using BlueSun.Data;
    using BlueSun.Models.NFTCollections;
    using System.Collections.Generic;

    public class NFTCollectionService : INFTCollectionService
    {
        private readonly BlueSunDbContext data;

        public NFTCollectionService(BlueSunDbContext data) 
            => this.data = data;

        public NFTCollectionQueryServiceModel All(
            string category,
            string searchTerm,
            CollectionSorting sorting,
            int currentPage,
            int collectionsPerPage)
        {
            var collectionsQuery = this.data.NFTCollections.AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
            {
                collectionsQuery = collectionsQuery.Where(c => c.Category.Name == category);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                collectionsQuery = collectionsQuery.Where(c =>
                c.Name.ToLower().Contains(searchTerm.ToLower()) ||
                c.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            var totalCollections = collectionsQuery.Count();

            collectionsQuery = sorting switch
            {
                CollectionSorting.DateCreated => collectionsQuery.OrderByDescending(c => c.Id),
                CollectionSorting.Name => collectionsQuery.OrderBy(c => c.Name),
                _ => collectionsQuery.OrderByDescending(c => c.Id)
            };

            var nftsCollections = collectionsQuery
                .Skip((currentPage - 1) * collectionsPerPage)
                .Take(collectionsPerPage)
                .Select(c => new NFTCollectionServiceModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImageUrl = c.ImageUrl,
                    Category = c.Category.Name
                })
                .ToList();

            return new NFTCollectionQueryServiceModel
            {
                TotalCollections = totalCollections,
                CurrentPage = currentPage,
                CollectionsPerPage = collectionsPerPage,
                Collections = nftsCollections
            };
        }

        public IEnumerable<string> AllCollectionCategories() 
            => this.data
                 .NFTCollections
                 .Select(c => c.Category.Name)
                 .OrderBy(c => c)
                 .Distinct()
                 .ToList();
    }
}

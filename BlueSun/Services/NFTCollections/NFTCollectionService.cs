namespace BlueSun.Services.NFTCollections
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using BlueSun.Data;
    using BlueSun.Data.Models;
    using BlueSun.Models.NFTCollections;
    using BlueSun.Services.NFTCollections.Models;
    using System.Collections.Generic;

    public class NFTCollectionService : INFTCollectionService
    {
        private readonly BlueSunDbContext data;
        private readonly IConfigurationProvider mapper;

        public NFTCollectionService(BlueSunDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

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

            var nftsCollections = GetCollections(collectionsQuery
                .Skip((currentPage - 1) * collectionsPerPage)
                .Take(collectionsPerPage));

            return new NFTCollectionQueryServiceModel
            {
                TotalCollections = totalCollections,
                CurrentPage = currentPage,
                CollectionsPerPage = collectionsPerPage,
                Collections = nftsCollections
            };
        }
        public IEnumerable<NFTCollectionServiceModel> ByUser(string userId)
        => GetCollections(this.data
            .NFTCollections
            .Where(c => c.Artist.UserId == userId));

        public bool IsByArtist(int collectionId, int artistId)
        => this.data
            .NFTCollections
            .Any(c => c.Id == collectionId && c.ArtistId == artistId);

        public IEnumerable<NFTCollectionCategoryServiceModel> AllCategories()
            => this.data
            .Categories
            .Select(c => new NFTCollectionCategoryServiceModel
            {
                Id = c.Id,
                Name = c.Name,
            })
            .ToList();

        private static IEnumerable<NFTCollectionServiceModel> GetCollections(IQueryable<NFTCollection> collectionQuery)
            => collectionQuery
                .Select(c => new NFTCollectionServiceModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImageUrl = c.ImageUrl,
                    CategoryName = c.Category.Name,
                })
            .ToList();

        public IEnumerable<LatestNFTCollectionServiceModel> Latest()
            => this.data
                .NFTCollections
                .OrderByDescending(n => n.Id)
                .ProjectTo<LatestNFTCollectionServiceModel>(this.mapper)
                .Take(3)
                .ToList();

        public NFTCollectionDetailsServiceModel Details(int collectionId)
        => this.data
            .NFTCollections
            .Where(c => c.Id == collectionId)
            .ProjectTo<NFTCollectionDetailsServiceModel>(this.mapper)
            .FirstOrDefault();

        public bool CategoryExists(int categoryId)
            => this.data.Categories.Any(c => c.Id == categoryId);

        public int Create(string name, string description, string imageUrl, int categoryId, int artistId)
        {
            var nftCollectionData = new NFTCollection
            {
                Name = name,
                Description = description,
                ImageUrl = imageUrl,
                CategoryId = categoryId,
                ArtistId = artistId
            };

            this.data.NFTCollections.Add(nftCollectionData);
            this.data.SaveChanges();

            return nftCollectionData.Id;
        }

        public bool NFTCollectionExists(string collectionName)
            => this.data.NFTCollections.Any(c => c.Name == collectionName);

        public bool Edit(int id, string name, string description, string imageUrl, int categoryId)
        {
            var nftCollectionData = this.data.NFTCollections.Find(id);

            if (nftCollectionData == null)
            {
                return false;
            }

            nftCollectionData.Name = name;
            nftCollectionData.Description = description;
            nftCollectionData.ImageUrl = imageUrl;
            nftCollectionData.CategoryId = categoryId;

            this.data.SaveChanges();

            return true;
        }

        
    }
}

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
            string category = null,
            string searchTerm = null,
            CollectionSorting sorting = CollectionSorting.DateCreated,
            int currentPage = 1,
            int collectionsPerPage = int.MaxValue,
            bool publicOnly = true)
        {
            var collectionsQuery = this.data.NFTCollections
                .Where(c => !publicOnly || c.IsPublic);

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

        public void ChangeVisibility(int id)
        {
            var collection = this.data.NFTCollections.Find(id);

            collection.IsPublic = !collection.IsPublic;

            this.data.SaveChanges();
        }



        public IEnumerable<LatestNFTCollectionServiceModel> Latest()
            => this.data
                .NFTCollections
                .Where(c => c.IsPublic)
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
        public IEnumerable<NFTCollectionCategoryServiceModel> AllCategories()
            => this.data
            .Categories
            .ProjectTo<NFTCollectionCategoryServiceModel>(this.mapper)
            .ToList();

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
                ArtistId = artistId,
                IsPublic = false
            };

            this.data.NFTCollections.Add(nftCollectionData);
            this.data.SaveChanges();

            return nftCollectionData.Id;
        }

        public bool NFTCollectionExists(string collectionName)
            => this.data.NFTCollections.Any(c => c.Name == collectionName);

        public bool Edit(
            int id,
            string name,
            string description, 
            string imageUrl,
            int categoryId,
            bool isPublic)
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
            nftCollectionData.IsPublic = false;

            this.data.SaveChanges();

            return true;
        }
        private IEnumerable<NFTCollectionServiceModel> GetCollections(IQueryable<NFTCollection> collectionQuery)
            => collectionQuery
            .ProjectTo<NFTCollectionServiceModel>(this.mapper)
            .ToList();
    }
}

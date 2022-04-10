namespace BlueSun.Infrastructure
{
    using AutoMapper;
    using BlueSun.Data.Models;
    using BlueSun.Models.NFTCollections;
    using BlueSun.Services.NFTCollections.Models;
    using BlueSun.Services.NFTs.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<ImportCollectionsModel, NFTCollection>();
            this.CreateMap<ImportNFTsModel, NFT>();

            this.CreateMap<Category, NFTCollectionCategoryServiceModel>();

            this.CreateMap<NFTCollection, LatestNFTCollectionServiceModel>();
            this.CreateMap<NFTCollectionDetailsServiceModel, NFTCollectionFormModel>();

            this.CreateMap<NFT, NFTListingServiceModel>();


            this.CreateMap<NFTCollection, NFTCollectionServiceModel>()
                .ForMember(c => c.CategoryName, cfg => cfg.MapFrom(c => c.Category.Name));


            this.CreateMap<NFTCollection, NFTCollectionDetailsServiceModel>()
                .ForMember(c => c.UserId, cfg => cfg.MapFrom(c => c.Artist.UserId))
                .ForMember(c => c.CategoryName, cfg => cfg.MapFrom(c => c.Category.Name));
        }
    }
}

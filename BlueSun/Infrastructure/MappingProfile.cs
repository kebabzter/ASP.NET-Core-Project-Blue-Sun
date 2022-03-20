﻿namespace BlueSun.Infrastructure
{
    using AutoMapper;
    using BlueSun.Data.Models;
    using BlueSun.Models.Home;
    using BlueSun.Models.NFTCollections;
    using BlueSun.Services.NFTCollections.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<NFTCollection, NFTCollectionIndexViewModel>();
            this.CreateMap<NFTCollectionDetailsServiceModel, NFTCollectionFormModel>();

            this.CreateMap<NFTCollection, NFTCollectionDetailsServiceModel>()
                .ForMember(c => c.UserId, cfg => cfg.MapFrom(c => c.Artist.UserId));
        }
    }
}
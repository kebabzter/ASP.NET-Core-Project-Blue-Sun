﻿namespace BlueSun.Data
{
    using BlueSun.Data.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    public class BlueSunDbContext : IdentityDbContext
    {
        public BlueSunDbContext(DbContextOptions<BlueSunDbContext> options)
            : base(options)
        {
        }

        public DbSet<NFT> NFTs { get; init; }

        public DbSet<Category> Categories { get; init; }

        //public DbSet<Artist> Artists { get; init; }

        //public DbSet<NFTCollection> NFTCollections { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<NFT>()
                .HasOne(c => c.Category)
                .WithMany(c=> c.NFTs)
                .HasForeignKey(c=>c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<NFT>()
            //    .HasOne(n => n.NFTCollection)
            //    .WithMany(n => n.NFTs)
            //    .HasForeignKey(n => n.NFTCollectionId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<NFTCollection>()
            //    .HasOne(n => n.Artist)
            //    .WithMany(a => a.NFTCollections)
            //    .HasForeignKey(n => n.ArtistId)
            //    .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}
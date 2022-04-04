namespace BlueSun.Data
{
    using BlueSun.Data.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    public class BlueSunDbContext : IdentityDbContext<User>
    {
        public BlueSunDbContext(DbContextOptions<BlueSunDbContext> options)
            : base(options)
        {
        }

        public DbSet<NFT> NFTs { get; init; }

        public DbSet<Category> Categories { get; init; }

        public DbSet<NFTCollection> NFTCollections { get; init; }

        public DbSet<Artist> Artists { get; init; }

        public DbSet<Wallet> Wallets { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Artist>()
                .HasOne<User>()
                .WithOne()
                .HasForeignKey<Artist>(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Wallet>()
                .HasOne(w => w.User)
                .WithOne(u => u.Wallet)
                .HasForeignKey<Wallet>(w => w.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Wallet>()
                .Property(w => w.Balance)
                .HasColumnType("decimal(7,2)");

            builder
                .Entity<NFT>()
                .HasOne(n => n.Owner)
                .WithMany(c => c.NFTs)
                .HasForeignKey(n => n.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<NFT>()
                .HasOne(c => c.Category)
                .WithMany(c => c.NFTs)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<NFTCollection>()
                .HasOne(c => c.Category)
                .WithMany(c => c.NFTCollections)
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<NFTCollection>()
                .HasOne(c => c.Artist)
                .WithMany(a => a.NFTCollections)
                .HasForeignKey(c => c.ArtistId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<NFT>()
                .HasOne(n => n.NFTCollection)
                .WithMany(n => n.NFTs)
                .HasForeignKey(n => n.NFTCollectionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<NFT>()
                .Property(n => n.Price)
                .HasColumnType("decimal(7,2)");

            base.OnModelCreating(builder);
        }
    }
}
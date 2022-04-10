using AutoMapper;
using BlueSun.Data;
using BlueSun.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

using static BlueSun.Areas.Admin.AdminConstants;

namespace BlueSun.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public  static IApplicationBuilder PrepareDatabase(
           this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);

            SeedCategories(services);
            SeedAdministrator(services);
            //SeedCollectionsNFTsUsers(services);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<BlueSunDbContext>();

            data.Database.Migrate();
        }

        private static void SeedCategories(IServiceProvider services)
        {
            var data = services.GetRequiredService<BlueSunDbContext>();

            if (data.Categories.Any())
            {
                return;
            }

            data.Categories.AddRange(new[]
            {
                new Category { Name = "Art"},
                new Category { Name = "Collectibles"},
                new Category { Name = "Photography"},
                new Category { Name = "Sports"},
            });

            data.SaveChanges();
        }

        private static void SeedAdministrator(
            IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task.Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdministratorRoleName))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = AdministratorRoleName };

                    await roleManager.CreateAsync(role);

                    const string adminEmail = "admin@bs.com";
                    const string adminPassword = "admin69";

                    var user = new User
                    {
                        Email = adminEmail,
                        UserName = adminEmail,
                        FullName = "Admin",
                        HasWallet = false,
                    };

                    await userManager.CreateAsync(user, adminPassword);

                    await userManager.AddToRoleAsync(user, role.Name);
                })
                .GetAwaiter()
                .GetResult();

            
        }

        //public static void SeedCollectionsNFTsUsers(IServiceProvider services)
        //{
        //    var data = services.GetRequiredService<BlueSunDbContext>();

        //    var userManager = services.GetRequiredService<UserManager<User>>();

        //    if (data.NFTCollections.Any() || data.NFTs.Any())
        //    {
        //        return;
        //    }

        //    var nftsJsonAsString = File.ReadAllText("./wwwroot/importNFTs.json");
        //    var nftCollectionsJsonAsString = File.ReadAllText("./wwwroot/importCollections.json");

        //    var collections = JsonConvert.DeserializeObject<IEnumerable<ImportCollectionsModel>>(nftCollectionsJsonAsString);
        //    var nfts = JsonConvert.DeserializeObject<IEnumerable<ImportNFTsModel>>(nftCollectionsJsonAsString);

        //    var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());

        //    var mapper = new Mapper(mapperConfiguration);

        //    var mappedCollections = mapper.Map<List<NFTCollection>>(collections);

        //    var mappedNFTs = mapper.Map<List<NFT>>(nfts);

        //    Task.Run(async () =>
        //    {
        //        if (userManager.Users.Count() > 1)
        //        {
        //            return;
        //        }

        //        const string userEmail = "user@bs.com";
        //        const string userName = "theG0dfather";
        //        const string password = "bs123";


        //        var user = new User
        //        {
        //            Email = userEmail,
        //            FullName = userName,
        //            HasWallet = true,
        //            Wallet = new Wallet { Balance = 10000 }
        //        };

        //        var artist = new Artist
        //        {
        //            Name = userName,
        //            UserId = user.Id,
        //            PhoneNumber = "+359-888-888-888"
        //        };

        //        data.Artists.Add(artist);

        //        await userManager.CreateAsync(user, password);

        //        mappedCollections.ForEach(c => c.ArtistId = artist.Id);

        //        mappedNFTs.ForEach(n => n.OwnerId = user.Id);

        //        data.NFTCollections.AddRange(mappedCollections);
        //        data.NFTs.AddRange(mappedNFTs);

        //        data.SaveChanges();
        //    })
        //        .GetAwaiter()
        //        .GetResult();

        //}
    }
}

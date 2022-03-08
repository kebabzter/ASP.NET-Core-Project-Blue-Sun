using BlueSun.Data;
using BlueSun.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BlueSun.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {
        public  static IApplicationBuilder PrepareDatabase(
           this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();


            var data = scopedServices.ServiceProvider.GetService<BlueSunDbContext>();

            data.Database.Migrate();

            SeedCategories(data);

            return app;
        }

        private static void SeedCategories(BlueSunDbContext data)
        {
            if (data.Categories.Any())
            {
                return;
            }

            data.Categories.AddRange(new[]
            {
                new Category { Name = "Art"},
                new Category { Name = "Collectibles"},
                new Category { Name = "Music"},
                new Category { Name = "Photography"},
                new Category { Name = "Sports"},
            });

            data.SaveChanges();
        }
    }
}

namespace BlueSun.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    public class BlueSunDbContext : IdentityDbContext
    {
        public BlueSunDbContext(DbContextOptions<BlueSunDbContext> options)
            : base(options)
        {
        }
    }
}
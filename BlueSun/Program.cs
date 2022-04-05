using BlueSun.Data;
using BlueSun.Data.Models;
using BlueSun.Infrastructure.Extensions;
using BlueSun.Services.Artists;
using BlueSun.Services.NFTCollections;
using BlueSun.Services.NFTs;
using BlueSun.Services.Statistics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BlueSunDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddDefaultIdentity<User>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BlueSunDbContext>();

//Was typeof(Program) 
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddMemoryCache();
builder.Services.AddSession();

builder.Services
    .AddControllersWithViews(options =>
    {
        options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
    });

builder.Services.AddTransient<IStatisticsService, StatisticsService>();
builder.Services.AddTransient<INFTCollectionService, NFTCollectionService>();
builder.Services.AddTransient<IArtistService, ArtistService>();
builder.Services.AddTransient<INFTsService, NFTsService>();

var app = builder.Build();

//Preparing DB from infrastructure folder
app.PrepareDatabase();

//Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection()
   .UseStaticFiles()
   .UseRouting()
   .UseAuthentication()
   .UseAuthorization()
   .UseSession();

app.MapDefaultAreaRoute();

app.MapControllerRoute(
    name: "NFTCollection Details",
    pattern: "/NFTCollections/Details/{id}/{information}",
    defaults: new { controller = "NFTCollections", action = "Details" });

app.MapDefaultControllerRoute();
app.MapRazorPages();

app.Run();

using BlueSun.Data;
using BlueSun.Data.Models;
using BlueSun.Infrastructure;
using BlueSun.Services.Artists;
using BlueSun.Services.NFTCollections;
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

builder.Services
    .AddControllersWithViews(options =>
    {
        options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
    });

builder.Services.AddTransient<IStatisticsService, StatisticsService>();
builder.Services.AddTransient<INFTCollectionService, NFTCollectionService>();
builder.Services.AddTransient<IArtistService, ArtistService>();



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
   .UseAuthorization();

app.MapDefaultAreaRoute();
app.MapDefaultControllerRoute();
app.MapRazorPages();

app.Run();

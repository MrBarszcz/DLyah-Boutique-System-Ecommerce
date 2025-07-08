using DLyah_Boutique_System;
using DLyah_Boutique_System.Data;
using DLyah_Boutique_System.Repository;
using DLyah_Boutique_System.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add database context, using SQL Server
builder.Services.AddDbContext<BankContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

// Add repository services
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>(); // Every time it is called, call the CategoryRepository
builder.Services.AddScoped<ISizeRepository, SizeRepository>();
builder.Services.AddScoped<IColorRepository, ColorRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IGenderRepository, GenderRepository>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<IBannerRepository, BannerRepository>();
builder.Services.AddScoped<IBannerPlacementRepository, BannerPlacementRepository>();
builder.Services.AddScoped<IBannerSlotService, BannerSlotService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.UseStaticFiles();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
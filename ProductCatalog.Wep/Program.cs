
using ProductCatalog.Core.Entities;
using ProductCatalog.Core.Interfaces;
using ProductCatalog.Infrastructure.Data;
using ProductCatalog.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Application.Interfaces;
using ProductCatalog.Application.Services;
using Microsoft.AspNetCore.Identity;
using Serilog;



namespace ProductCatalog.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Add Identity service
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Configure DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("ProductCatalog"),
                    b => b.MigrationsAssembly("ProductCatalog.Infrastructure")
                )
            );

            // Register repositories and services

            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();


            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IProductUpdateLog, ProductUpdateLog>();




          

            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();


            //   Configure Serilog
            Log.Logger = new LoggerConfiguration()
                 .ReadFrom.Configuration(config)
                .WriteTo.Console()
                .WriteTo.File("logs/lognew.txt") // Log to a file
                .CreateLogger();

          

            builder.Host.UseSerilog(); // Use Serilog for logging


            Log.Information("Starting up the application");

            var app = builder.Build();

            // Seed roles and admin user            //using (var scope = app.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    await SeedData.Initialize(services);
            //}




            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(); 
            app.UseRouting();

            app.UseAuthentication(); 
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Products}/{action=IndexOnTime}/{id?}");

            app.Run();
        }
    }
}
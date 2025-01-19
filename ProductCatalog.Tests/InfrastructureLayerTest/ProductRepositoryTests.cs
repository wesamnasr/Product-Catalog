using Microsoft.EntityFrameworkCore;
using ProductCatalog.Core.Entities;
using ProductCatalog.Infrastructure.Data;
using ProductCatalog.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProductCatalog.Tests.InfrastructureLayerTest.Repositories
{
    public class ProductRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ProductRepository _repository;

        public ProductRepositoryTests()
        {
            
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new ProductRepository(_context);

           
            SeedData();
        }

        private void SeedData()
        {
            // Add test categories
            _context.Categories.AddRange(
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Clothing" }
            );

            // Add test products
            _context.Products.AddRange(
                new Product { Id = 1, Name = "Laptop", CategoryId = 1, Price = 1000, Duration = 10, StartDate = DateTime.UtcNow, CreatedByUserId = "1",Description="",ImagePath=""},
                new Product { Id = 2, Name = "T-Shirt", CategoryId = 2, Price = 20, Duration = 5, StartDate = DateTime.UtcNow.AddDays(-10), CreatedByUserId = "1", Description = "", ImagePath = "" },
                new Product { Id = 3, Name = "Smartphone", CategoryId = 1, Price = 500, Duration = 7, StartDate = DateTime.UtcNow.AddDays(-5), CreatedByUserId = "1", Description = "", ImagePath = "" }
            );

            _context.SaveChanges();
        }

        public void Dispose()
        {
            
            _context.Dispose();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            // Act
            var products = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(3, products.Count()); 
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProduct_WhenProductExists()
        {
            // Act
            var product = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(product);
            Assert.Equal("Laptop", product.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Act
            var product = await _repository.GetByIdAsync(01100699524); // Non-existent ID

            // Assert
            Assert.Null(product);
        }

        [Fact]
        public async Task AddAsync_ShouldAddProductToDatabase()
        {
            // Arrange
            var newProduct = new Product
            {
                Name = "Tablet",
                CategoryId = 1,
                Price = 300,
                Duration = 8,
                StartDate = DateTime.UtcNow,
                CreatedByUserId = "1",
                Description="",
                ImagePath = ""
            };

            // Act
            await _repository.AddAsync(newProduct);

            // Assert
            var productInDb = await _context.Products.FirstOrDefaultAsync(p => p.Name == "Tablet");
            Assert.NotNull(productInDb);
           
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateProductInDatabase()
        {
            // Arrange
            var product = await _repository.GetByIdAsync(1);
            product.Price = 1200; // Update the price

            // Act
            await _repository.UpdateAsync(product);

            // Assert
            var updatedProduct = await _repository.GetByIdAsync(1);
            Assert.Equal(1200, updatedProduct.Price);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveProduct_WhenProductExists()
        {
            // Act
            await _repository.DeleteAsync(1);

            // Assert
            var productInDb = await _repository.GetByIdAsync(1);
            Assert.Null(productInDb); // Product should be deleted
        }

        [Fact]
        public async Task GetActiveProductsAsync_ShouldReturnActiveProducts()
        {
            // Act
            var activeProducts = await _repository.GetActiveProductsAsync();

            // Assert
            Assert.Equal(2, activeProducts.Count()); 
            Assert.Contains(activeProducts, p => p.Name == "Laptop");
            Assert.Contains(activeProducts, p => p.Name == "Smartphone");
        }

        [Fact]
        public async Task GetCategoryNameByIdAsync_ShouldReturnCategoryName_WhenCategoryExists()
        {
            // Act
            var categoryName = await _repository.GetCategoryNameByIdAsync(1);

            // Assert
            Assert.Equal("Electronics", categoryName);
        }

        [Fact]
        public async Task GetCategoryNameByIdAsync_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            // Act
            var categoryName = await _repository.GetCategoryNameByIdAsync(999); // Non-existent ID

            // Assert
            Assert.Null(categoryName);
        }
    }
}
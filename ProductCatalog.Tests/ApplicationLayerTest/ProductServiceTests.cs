using Microsoft.Extensions.Logging;
using Moq;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Interfaces;
using ProductCatalog.Application.Services;
using ProductCatalog.Core.Entities;
using ProductCatalog.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Tests.ApplicationLayerTest
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<ILogger<ProductService>> _mockLogger;
        private readonly Mock<IProductUpdateLog> _mockProductUpdateLog;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _mockLogger = new Mock<ILogger<ProductService>>();
            _productService = new ProductService(
                _mockProductRepository.Object,
                _mockLogger.Object,
                _mockProductUpdateLog.Object);
        }



        [Fact]
        public async Task GetActiveProductsAsync_ShouldReturnActiveProducts()
        {
            // Arrange
            var activeProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", StartDate = DateTime.UtcNow, Duration = 10 },
                new Product { Id = 2, Name = "Smartphone", StartDate = DateTime.UtcNow.AddDays(-5), Duration = 7 }
            };

            _mockProductRepository
                .Setup(repo => repo.GetActiveProductsAsync())
                .ReturnsAsync(activeProducts);

            // Act
            var result = await _productService.GetActiveProductsAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, p => p.Name == "Laptop");
            Assert.Contains(result, p => p.Name == "Smartphone");
        }



        [Fact]
        public async Task GetActiveProductsByCategoryAsync_ShouldReturnFilteredProducts()
        {
            // Arrange
            var activeProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", CategoryId = 1, StartDate = DateTime.UtcNow, Duration = 10 },
                new Product { Id = 2, Name = "Smartphone", CategoryId = 1, StartDate = DateTime.UtcNow.AddDays(-5), Duration = 7 },
                new Product { Id = 3, Name = "T-Shirt", CategoryId = 2, StartDate = DateTime.UtcNow.AddDays(-10), Duration = 5 }
            };

            _mockProductRepository
                .Setup(repo => repo.GetActiveProductsAsync())
                .ReturnsAsync(activeProducts);

            // Act
            var result = await _productService.GetActiveProductsByCategoryAsync(1);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, p => p.Name == "Laptop");
            Assert.Contains(result, p => p.Name == "Smartphone");
        }


        [Fact]
        public async Task GetProductsAsync_ShouldReturnAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", CategoryId = 1 },
                new Product { Id = 2, Name = "Smartphone", CategoryId = 1 },
                new Product { Id = 3, Name = "T-Shirt", CategoryId = 2 }
            };

            _mockProductRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(products);

            // Act
            var result = await _productService.GetProductsAsync(null);

            // Assert
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Laptop" };

            _mockProductRepository
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(product);

            // Act
            var result = await _productService.GetProductByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            _mockProductRepository
                .Setup(repo => repo.GetByIdAsync(999))
                .ReturnsAsync((Product)null);

            // Act
            var result = await _productService.GetProductByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddProductAsync_ShouldAddProduct()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Name = "Laptop",
                CategoryId = 1,
                Price = 1000,
                Duration = 10,
                StartDate = DateTime.UtcNow
            };

            _mockProductRepository
                .Setup(repo => repo.GetCategoryNameByIdAsync(1))
                .ReturnsAsync("Electronics");

            _mockProductRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Product>()))
                .Returns(Task.CompletedTask);

            // Act
            await _productService.AddProductAsync(productDto);

            // Assert
            _mockProductRepository.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldUpdateProduct()
        {
            // Arrange
            var productDto = new ProductDto
            {
                Id = 1,
                Name = "Laptop",
                CategoryId = 1,
                Price = 1200,
                Duration = 10,
                StartDate = DateTime.UtcNow
            };

            var existingProduct = new Product { Id = 1, Name = "Laptop" };

            _mockProductRepository
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(existingProduct);

            _mockProductRepository
                .Setup(repo => repo.GetCategoryNameByIdAsync(1))
                .ReturnsAsync("Electronics");

            _mockProductRepository
                .Setup(repo => repo.UpdateAsync(It.IsAny<Product>()))
                .Returns(Task.CompletedTask);

            // Act
            await _productService.UpdateProductAsync(productDto);

            // Assert
            _mockProductRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task DeleteProductAsync_ShouldDeleteProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Laptop" };

            _mockProductRepository
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(product);

            _mockProductRepository
                .Setup(repo => repo.DeleteAsync(1))
                .Returns(Task.CompletedTask);

            // Act
            await _productService.DeleteProductAsync(1);

            // Assert
            _mockProductRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }
    }
}
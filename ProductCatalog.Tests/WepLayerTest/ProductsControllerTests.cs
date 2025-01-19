using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Interfaces;
using ProductCatalog.Core.Entities;
using ProductCatalog.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Tests.WepLayerTest
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly Mock<ICategoryService> _mockCategoryService;
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<ILogger<ProductsController>> _mockLogger;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mockProductService = new Mock<IProductService>();
            _mockCategoryService = new Mock<ICategoryService>();
            _mockLogger = new Mock<ILogger<ProductsController>>();

            _mockUserManager = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

                _controller = new ProductsController(
                _mockProductService.Object,
                _mockUserManager.Object,
                _mockCategoryService.Object,
                _mockLogger.Object);
        }


        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfProducts()
        {
            // Arrange
            var categories = new List<CategoryDto>
            {
                new CategoryDto { Id = 1, Name = "Test Category 1" },
                new CategoryDto { Id = 2, Name = "Test Category 2" },
                new CategoryDto { Id = 3, Name = "Test Category 3" }
            };
         
            var products = new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Test Product 1", Price = 10, CategoryId = 1 },
                new ProductDto { Id = 2, Name = "Test Product 2", Price = 20, CategoryId = 2 },
                new ProductDto { Id = 3, Name = "Test Product 3", Price = 30, CategoryId = 3 }
            };

            _mockCategoryService
                .Setup(service => service.GetAllCategoriesAsync())
                .ReturnsAsync(categories);
         
            _mockProductService
             .Setup(service => service.GetProductsAsync(It.IsAny<int?>()))
             .ReturnsAsync(products);

            // Act
            var result = await _controller.Index(null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(viewResult.Model);
            Assert.Equal(3, model.Count());
        }

        [Fact]
        public async Task IndexOnTime_ShouldReturnViewWithActiveProducts()
        {
            // Arrange
            var categories = new List<CategoryDto>
            {
                new CategoryDto { Id = 1, Name = "Test Category 1" },
                new CategoryDto { Id = 2, Name = "Test Category 2" },
                new CategoryDto { Id = 3, Name = "Test Category 3" }
            };
            var products = new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Test Product 1", Price = 10, CategoryId = 1 },
                new ProductDto { Id = 2, Name = "Test Product 2", Price = 20, CategoryId = 2 },
                new ProductDto { Id = 3, Name = "Test Product 3", Price = 30, CategoryId = 3 }
            };

            _mockCategoryService
                .Setup(service => service.GetAllCategoriesAsync())
                .ReturnsAsync(categories);
         
           
            _mockProductService.Setup(service=> service.GetActiveProductsByCategoryAsync(It.IsAny<int?>()))
                .ReturnsAsync(products);

            // Act
            var result = await _controller.IndexOnTime(null);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(viewResult.Model);
            Assert.Equal(3, model.Count());



        }

        [Fact]
        public async Task Details_ShouldReturnViewWithProductDetails()
        {
            // Arrange
            var product = new ProductDto
            {
                Id = 1,
                Name = "Test Product 1",
                Price = 10,
                CategoryId = 1
            };
            _mockProductService
                .Setup(service => service.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(product);
            // Act
            var result = await _controller.Details(1);
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductDto>(viewResult.Model);
            Assert.Equal(1, model.Id);
        }

        [Fact]
        public async Task Details_ShouldReturnNotFound_WhenProductNotExits()
        {
            // Arrange
            _mockProductService
                .Setup(service => service.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((ProductDto)null);
            // Act
            var result = await _controller.Details(1);
            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateOrEdit_Get_ShouldReturnViewWithProducts()
        {
            // Arrange
          
            var product = new ProductDto { Id = 1, Name = "Test Product 1", Price = 10, CategoryId = 1 };

            _mockProductService
                 .Setup(service => service.GetProductByIdAsync(It.IsAny<int>()))
                 .ReturnsAsync(product);
         

            // Act
            var result = await _controller.CreateOrEdit(1);
            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductDto>(viewResult.Model);
            Assert.Equal(1, model.Id);
        }

        [Fact]
        public async Task CreateOrEdit_Post_ShouldRedirectToIndex_WhenModelIsValid()
        {
            // Arrange
            var product = new ProductDto { Id = 1, Name = "Test Product 1", Price = 10, CategoryId = 1 };
           
            
            _mockProductService
               .Setup(service => service.AddProductAsync(It.IsAny<ProductDto>()))
               .Returns(Task.CompletedTask);

            _mockUserManager.Setup(Manager => Manager.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns("1");  

            // Act
            var result = await _controller.CreateOrEdit(null,product);
            
            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);   

        }

        [Fact]
        public async Task DeleteConfimed_ShouldRedirectToIndex_WhenProductDeleted()
        {
            // Arrange
            _mockProductService
                .Setup(service => service.DeleteProductAsync(It.IsAny<int>()))
                .Returns(Task.CompletedTask);
            // Act
            var result = await _controller.DeleteConfirmed(1);
            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }




    }
}

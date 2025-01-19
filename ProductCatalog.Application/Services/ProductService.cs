using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Interfaces;
using ProductCatalog.Core.Entities;
using ProductCatalog.Core.Interfaces;
using ProductCatalog.Infrastructure.Data;
using ProductCatalog.Infrastructure.Repositories;

namespace ProductCatalog.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;
        private readonly IProductUpdateLog _productUpdateLog;

        public ProductService(IProductRepository productRepository,ILogger<ProductService>logger, IProductUpdateLog productUpdateLog)
        {
            _productRepository = productRepository;
            _logger = logger;
            _productUpdateLog= productUpdateLog;
        }

        public async Task<IEnumerable<ProductDto>> GetActiveProductsAsync()
        {
            try
            {
                var activeProducts = await _productRepository.GetActiveProductsAsync();
                _logger.LogInformation("Active products retrieved.");

                return activeProducts.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    CreationDate = p.CreationDate,
                    CreatedByUserId = p.CreatedByUserId,
                    StartDate = p.StartDate,
                    Duration = p.Duration,
                    Price = p.Price,
                    ImagePath = p.ImagePath,
                    CategoryId = p.CategoryId,
                    CategoryName = p.CategoryName,  
                });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed to get active products.");
                throw;
            }
        }


        public async Task<IEnumerable<ProductDto>> GetActiveProductsByCategoryAsync(int? categoryId)
        {
            try
            {
                var activeProducts = await _productRepository.GetActiveProductsAsync();

                if (categoryId.HasValue)
                {
                    activeProducts = activeProducts.Where(p => p.CategoryId == categoryId.Value).ToList();
                }
                _logger.LogInformation("Active products retrieved by category.");
                return activeProducts.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    CreationDate = p.CreationDate,
                    CreatedByUserId = p.CreatedByUserId,
                    StartDate = p.StartDate,
                    Duration = p.Duration,
                    Price = p.Price,
                    ImagePath = p.ImagePath,
                    CategoryId = p.CategoryId,
                    Description = p.Description,
                    CategoryName = p.CategoryName

                });
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed to get active products by category.");
                throw;
            }
        }


        public async Task<IEnumerable<ProductDto>> GetProductsAsync(int? categoryId)
        {
            try
            {
                var products = await _productRepository.GetAllAsync();


                if (categoryId.HasValue)
                {
                    products = products.Where(p => p.CategoryId == categoryId.Value).ToList();
                }
                var productDtos = new List<ProductDto>();


                foreach (var product in products)
                {
                    productDtos.Add(new ProductDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        CreationDate = product.CreationDate,
                        CreatedByUserId = product.CreatedByUserId,
                        StartDate = product.StartDate,
                        Duration = product.Duration,
                        Price = product.Price,
                        ImagePath = product.ImagePath,
                        CategoryId = product.CategoryId,
                        CategoryName = product.CategoryName

                    });
                }
                _logger.LogInformation("Products retrieved.");

                return productDtos;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed to get products.");
                throw;
            }
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null)
                {
                    return null;
                }
                _logger.LogInformation("Product retrieved with ID: {ProductId}", id);
                return new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    CreationDate = product.CreationDate,
                    CreatedByUserId = product.CreatedByUserId,
                    StartDate = product.StartDate,
                    Duration = product.Duration,
                    Price = product.Price,
                    ImagePath = product.ImagePath,
                    CategoryId = product.CategoryId,
                    CategoryName = product.CategoryName
                };

            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed to get product with ID: {ProductId}", id);
                throw;
            }
        }

        public async Task AddProductAsync(ProductDto productDto)
        {
            try
          

        {
                productDto.CategoryName = (await _productRepository.GetCategoryNameByIdAsync(productDto.CategoryId));
                productDto.CreationDate = System.DateTime.Now;
                var product = new Product
                {
                    Name = productDto.Name,
                    Description = productDto.Description,
                    CreationDate = productDto.CreationDate,
                    CreatedByUserId = productDto.CreatedByUserId,
                    StartDate = productDto.StartDate,
                    Duration = productDto.Duration,
                    Price = productDto.Price,
                    ImagePath = productDto.ImagePath,
                    CategoryId = productDto.CategoryId,
                    CategoryName = productDto.CategoryName
                };

                product.Validate(); // Validate product properties
                await _productRepository.AddAsync(product);
                _logger.LogInformation("Product added: {ProductName} by User ID: {UserId}", productDto.Name, productDto.CreatedByUserId);
            }
            catch (System.Exception ex)
            {

                _logger.LogError(ex, "Failed to add product: {ProductName}", productDto.Name);
                throw;
            }
        }

        public async Task UpdateProductAsync(ProductDto productDto)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productDto.Id);
                if (product == null)
                {
                    throw new KeyNotFoundException("Product not found.");
                }
                var oldValues = JsonSerializer.Serialize(product);
               
                
                productDto.CategoryName = (await _productRepository.GetCategoryNameByIdAsync(productDto.CategoryId));

                product.Name = productDto.Name;
                product.Description = productDto.Description;
                product.CreationDate = productDto.CreationDate;
                product.CreatedByUserId = productDto.CreatedByUserId;
                product.StartDate = productDto.StartDate;
                product.Duration = productDto.Duration;
                product.Price = productDto.Price;
                product.ImagePath = productDto.ImagePath;
                product.CategoryId = productDto.CategoryId;
                product.CategoryName=productDto.CategoryName;

                product.Validate(); // Validate product properties
                await _productRepository.UpdateAsync(product);


                var NewValues = JsonSerializer.Serialize(productDto);


                // logging the updates in a separate table in data base

                await _productUpdateLog.LogProductUpdateAsync(productDto.Id, productDto.CreatedByUserId,oldValues,NewValues);


                _logger.LogInformation("Product updated: {ProductName} by User ID: {UserId}", productDto.Name, productDto.CreatedByUserId);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed to update product: {ProductName}", productDto.Name);
                throw;
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null)
                {
                    throw new KeyNotFoundException("Product not found.");
                }

                await _productRepository.DeleteAsync(id);
                _logger.LogInformation("Product deleted with ID: {ProductId}", id);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed to delete product with ID: {ProductId}", id);
                throw;
            }
        }

        private readonly string _uploadDirectory = "wwwroot/images";
        public async Task<string> HandleImageUploadAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                throw new ArgumentException("Image file is invalid.");
            }

            var fileExtension = Path.GetExtension(imageFile.FileName).ToLower();
            if (!new[] { ".jpg", ".jpeg", ".png" }.Contains(fileExtension))
            {
                throw new ArgumentException("Only JPG, JPEG, and PNG files are allowed.");
            }

            if (imageFile.Length > 1024 * 1024) 
            {
                throw new ArgumentException("The image file cannot exceed 1MB.");
            }

            var fileName = Guid.NewGuid().ToString() + fileExtension;
            var filePath = Path.Combine(_uploadDirectory, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/images/{fileName}";
        }

        public async Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm)
        {
            try
            {
                var products = await _productRepository.SearchProductsAsync(searchTerm);
                return products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    CreationDate = p.CreationDate,
                    CreatedByUserId = p.CreatedByUserId,
                    StartDate = p.StartDate,
                    Duration = p.Duration,
                    Price = p.Price,
                    ImagePath = p.ImagePath,
                    CategoryId = p.CategoryId,
                    CategoryName = p.CategoryName
                });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to search products with Terms{searchTerm}.", searchTerm);
                throw;
            }

        }
    }
}
using Microsoft.AspNetCore.Http;
using ProductCatalog.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync(int? categoryId);
        Task<IEnumerable<ProductDto>> GetActiveProductsAsync();

        Task<IEnumerable<ProductDto>> GetActiveProductsByCategoryAsync(int? categoryId);
        Task<ProductDto> GetProductByIdAsync(int id);
        Task AddProductAsync(ProductDto productDto);
        Task UpdateProductAsync(ProductDto productDto);
        Task DeleteProductAsync(int id);
        Task<string> HandleImageUploadAsync(IFormFile imageFile);

        Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm);
    }
}

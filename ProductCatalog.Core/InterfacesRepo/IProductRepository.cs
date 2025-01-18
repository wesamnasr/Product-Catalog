using ProductCatalog.Core.Entities;



namespace ProductCatalog.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<string?> GetCategoryNameByIdAsync(int categoryId);
        Task<IEnumerable<Product>> GetActiveProductsAsync(); 
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product); 
        Task UpdateAsync(Product product); 
        Task DeleteAsync(int id);

        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
    }
}

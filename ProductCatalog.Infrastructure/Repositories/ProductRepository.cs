﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Core.Entities;
using ProductCatalog.Core.Interfaces;
using ProductCatalog.Infrastructure.Data;

namespace ProductCatalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetActiveProductsAsync()
        {
            var currentTime = DateTime.UtcNow; 

            return await _context.Products
                .Where(p => p.StartDate <= currentTime && p.StartDate.AddDays(p.Duration) >= currentTime)
                .ToListAsync();
        }
        public async Task<string?> GetCategoryNameByIdAsync(int categoryId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            return category?.Name;
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
           return await _context.Products
                .Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
                .ToListAsync();
        }
    }
}
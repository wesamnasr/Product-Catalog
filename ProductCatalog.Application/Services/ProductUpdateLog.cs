using Microsoft.Extensions.Logging;
using ProductCatalog.Application.Interfaces;
using ProductCatalog.Core.Entities;
using ProductCatalog.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Application.Services
{
    public class ProductUpdateLog : IProductUpdateLog
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductUpdateLog> _logger;

        public ProductUpdateLog(ApplicationDbContext context, ILogger<ProductUpdateLog> logger)
        {
            _context = context;
            _logger = logger;
        }
        public Task LogProductUpdateAsync(int productId, string updatedBy, string oldValue, string newValue)
        {
            var log = new ProductUpdateLogEn
            {
               ProductId = productId,
               UpdatedBy = updatedBy,
               OldValues = oldValue,
               NewValues = newValue,
               UpdatedAt = DateTime.Now
           };
            _context.productUpdateLogEns.Add(log);
            return _context.SaveChangesAsync();

        }
    }
}

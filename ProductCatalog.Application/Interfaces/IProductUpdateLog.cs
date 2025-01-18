using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Application.Interfaces
{
    public interface IProductUpdateLog
    {
        public  Task LogProductUpdateAsync(int productId, string updatedBy, string oldValue, string newValue);
    }
}

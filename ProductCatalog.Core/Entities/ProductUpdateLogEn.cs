using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Core.Entities
{
    public class ProductUpdateLogEn
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
    }
}

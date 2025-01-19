using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Core.Exceptions
{
    public class InvalidStartDateException : Exception
    {
        public InvalidStartDateException(string message) : base(message) { }
    }
}

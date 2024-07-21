using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductShop.DTOs.Export
{
    public class SellerWithProductDto
    {
        public string FirstName { get; set; }   

        public string LastName { get; set; }

        public IEnumerable<SoldProductDto> SoldProducts { get; set; }
    }
}

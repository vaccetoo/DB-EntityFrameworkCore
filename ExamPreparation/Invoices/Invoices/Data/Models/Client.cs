using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Data.Models
{
    public class Client
    {
        public Client()
        {
            Invoices = new List<Invoice>();
            Addresses = new List<Address>();
            ProductsClients = new List<ProductClient>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(10), MaxLength(25)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(10), MaxLength(15)]
        public string NumberVat { get; set; } = null!;

        public ICollection<Invoice> Invoices { get; set; }

        public ICollection<Address> Addresses { get; set; }

        public ICollection<ProductClient> ProductsClients { get; set; }
    }
}

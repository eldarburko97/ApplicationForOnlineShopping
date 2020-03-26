using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Models
{
    public class CartDetails
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int Quantity { get; set; }
        public int ProductSizeId { get; set; }
        public ProductSize ProductSize { get; set; }
    }
}

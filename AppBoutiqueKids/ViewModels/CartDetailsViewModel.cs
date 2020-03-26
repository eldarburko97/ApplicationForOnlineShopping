using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.ViewModels
{
    public class CartDetailsViewModel
    {
        public int CartDetailsId { get; set; }
        public string PhotoPath { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}

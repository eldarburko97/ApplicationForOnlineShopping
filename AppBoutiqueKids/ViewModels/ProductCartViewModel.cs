using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.ViewModels
{
    public class ProductCartViewModel
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Category { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string PhotoPath { get; set; }
        public int ProductSizeId { get; set; }
        public List<SelectListItem> ProductSizes { get; set; }
        public int UserId { get; set; }
    }
}

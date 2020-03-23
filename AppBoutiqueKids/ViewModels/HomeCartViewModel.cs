using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.ViewModels
{
    public class HomeCartViewModel
    {
        public int Id { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public string Photo { get; set; }
        public string ProductName { get; set; }
        public float Total { get; set; }
        public float FinalTotal { get; set; }
    }
}

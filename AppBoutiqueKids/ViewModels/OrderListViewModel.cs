using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.ViewModels
{
    public class OrderListViewModel
    {
        public int Id { get; set; }
        public string User { get; set; }
        public string Email { get; set; }
        public string FirstLastName { get; set; }
        public string CityAdressZipCode { get; set; }
        public string Phone { get; set; }
        public string Product { get; set; }
        public string Size { get; set; }
    }
}

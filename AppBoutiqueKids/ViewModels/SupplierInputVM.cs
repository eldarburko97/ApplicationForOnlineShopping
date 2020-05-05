using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.ViewModels
{
    public class SupplierInputVM
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; }
        [Required]
        [Phone]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }
    }
}

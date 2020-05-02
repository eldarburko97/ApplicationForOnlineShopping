using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.ViewModels
{
    public class SizeInputVM
    {
        public int Id { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(5)]
        public string Name { get; set; }

    }
}

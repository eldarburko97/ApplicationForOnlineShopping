using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Models
{
    public class OrderDetails
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int Price { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int Discount { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int DiscountPrice { get; set; }

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Orders { get; set; }

        public int ProductSizeId { get; set; }
        [ForeignKey("ProductSizeId")]
        public ProductSize Product { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Models
{
    public class SupplierProducts
    {
        [Key]
        public int SupplierProductsId { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public int SupplierId { get; set; }
        [ForeignKey("SupplierId")]

        public Supplier Supplier { get; set; }

        

    }
}

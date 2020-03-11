using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Models
{
    public class Supplier_Shipper
    {
        [Key]
        public int SupplierShipperId { get; set; }
        
       
        public int SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public Supplier Supplier { get; set; }
        public int ShipperId { get; set; }
        
        [ForeignKey("ShipperId")]
        
        public Shipper Shipper { get; set; }

    }
}

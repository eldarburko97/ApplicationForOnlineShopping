using AppBoutiqueKids.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Services
{
    public interface ISupplier
    {
        List<Supplier> GetSuppliers();
        Supplier AddSupplier(Supplier supplier);
        Supplier UpdateSupplier(Supplier supplier);
        Supplier DeleteSupplier(int Id);
        Supplier GetSupplier(int Id);
    }
}

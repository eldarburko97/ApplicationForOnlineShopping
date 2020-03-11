using AppBoutiqueKids.Data;
using AppBoutiqueKids.Models;
using AppBoutiqueKids.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Repository
{
    public class SupplierRepository : ISupplier
    {
        private ApplicationDbContext _context;

        public SupplierRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Supplier AddSupplier(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            _context.SaveChanges();
            return supplier;
        }

        public Supplier DeleteSupplier(int Id)
        {
            Supplier supplier = _context.Suppliers.Find(Id);
            _context.Suppliers.Remove(supplier);
            _context.SaveChanges();
            return supplier;
        }

        public Supplier GetSupplier(int Id)
        {
            return _context.Suppliers.Find(Id);
        }

        public List<Supplier> GetSuppliers()
        {
            return _context.Suppliers.ToList();
        }

        public Supplier UpdateSupplier(Supplier supplier)
        {
            _context.Suppliers.Update(supplier);
            _context.SaveChanges();
            return supplier;
        }
    }
}

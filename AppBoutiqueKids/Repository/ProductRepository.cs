using AppBoutiqueKids.Data;
using AppBoutiqueKids.Models;
using AppBoutiqueKids.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Repository
{
    public class ProductRepository : IProduct
    {
        private ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Product AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }

        public Product DeleteProduct(int Id)
        {
            Product product = _context.Products.Find(Id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return product;
        }

        public Product GetProduct(int Id)
        {
            return _context.Products.Include(b=>b.Brand).Include(c=>c.Category).FirstOrDefault(p=>p.Id==Id);
        }

        public List<Product> GetProducts()
        {
            return _context.Products.ToList();
        }

        public List<Product> SearchProducts(string searchTerm)
        {
          if(string.IsNullOrEmpty(searchTerm))
            {
                return _context.Products.ToList();
            }
            return _context.Products.Where(w => w.Name.Contains(searchTerm)).ToList();
        }

        public Product UpdateProduct(Product product)
        {
          _context.Products.Update(product);
            _context.SaveChanges();
            return product;
        }
    }
}

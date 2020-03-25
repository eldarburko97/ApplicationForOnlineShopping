using AppBoutiqueKids.Data;
using AppBoutiqueKids.Models;
using AppBoutiqueKids.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Repository
{
    public class ProductSizeRepository : IProductSize
    {
        private ApplicationDbContext _context;

        public ProductSizeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public ProductSize Add(ProductSize model)
        {
            _context.ProductSizes.Add(model);
            _context.SaveChanges();
            return model;
        }
       

        public ProductSize GetProductSize(int id)
        {
            var productSize = _context.ProductSizes.Find(id);
            return productSize;
        }
    }
}

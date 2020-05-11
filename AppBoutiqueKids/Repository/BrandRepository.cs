using AppBoutiqueKids.Data;
using AppBoutiqueKids.Models;
using AppBoutiqueKids.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Repository
{
    public class BrandRepository : IBrand
    {
        private ApplicationDbContext _context;

        public BrandRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Brand> GetBrands() => _context.Brands.ToList();
        Brand IBrand.AddBrand(Brand brand)
        {
            _context.Brands.Add(brand);
            _context.SaveChanges();
            return brand;
        }
        Brand IBrand.GetBrand(int id)
        {
            Brand brand=_context.Brands.Find(id);
            return brand;
        }
        Brand IBrand.DeleteBrand(int id)
        {
            Brand deleteBrand = _context.Brands.Find(id);
            if (deleteBrand != null)
            {
                _context.Brands.Remove(deleteBrand);
                _context.SaveChanges();
            }
            return deleteBrand;
        }
        Brand IBrand.UpdateBrand(Brand model)
        {
            Brand updateBrand = _context.Brands.Find(model.Id);
            if (updateBrand != null)
            {
                updateBrand.Name = model.Name;
                _context.Brands.Update(updateBrand);
                _context.SaveChanges();
            }
            return updateBrand;
        }
    }
}

using AppBoutiqueKids.Data;
using AppBoutiqueKids.Models;
using AppBoutiqueKids.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Repository
{
    public class CategoryRepository : ICategory
    {
        private ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Category AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return category;
        }

        public Category DeleteCategory(int Id)
        {
            Category category = _context.Categories.Find(Id);
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return category;
        }

        public List<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategory(int Id)
        {
            return _context.Categories.Find(Id);
        }

        public Category UpdateCategory(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
            return category;
        }
    }
}

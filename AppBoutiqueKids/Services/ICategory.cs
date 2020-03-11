using AppBoutiqueKids.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Services
{
    public interface ICategory
    {
        List<Category> GetCategories();
        Category AddCategory(Category category);
        Category UpdateCategory(Category category);
        Category DeleteCategory(int Id);
        Category GetCategory(int Id);
    }
}

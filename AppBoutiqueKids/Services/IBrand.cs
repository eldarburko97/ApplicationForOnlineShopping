using AppBoutiqueKids.Models;
using AppBoutiqueKids.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Services
{
    public interface IBrand
    {
        Brand AddBrand(Brand brand);
        Brand DeleteBrand(int id);
        Brand UpdateBrand(Brand model);
        Brand GetBrand(int id);
        IEnumerable<Brand> GetBrands();
    }
}

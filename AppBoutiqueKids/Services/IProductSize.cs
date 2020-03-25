using AppBoutiqueKids.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Services
{
    public interface IProductSize
    {
        ProductSize Add(ProductSize model);
        ProductSize GetProductSize(int id);
    }
}

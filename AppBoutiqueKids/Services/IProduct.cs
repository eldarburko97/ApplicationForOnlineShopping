using AppBoutiqueKids.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Services
{
    public interface IProduct
    {
        List<Product> GetProducts();
        Product AddProduct(Product product);
        Product UpdateProduct(Product product);
        Product DeleteProduct(int Id);
        Product GetProduct(int Id);
        List<Product> SearchProducts(string searchTerm);
    }
}

using AppBoutiqueKids.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Services
{
    public interface ICartDetails
    {
        CartDetails Add(CartDetails model);
        CartDetails GetCartDetail(int id);
        List<CartDetails> GetCartDetails();
        CartDetails DeleteCartDetail(int id);
        CartDetails UpdateCartDetail(CartDetails model);
    }
}

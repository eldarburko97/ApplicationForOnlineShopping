using AppBoutiqueKids.Data;
using AppBoutiqueKids.Models;
using AppBoutiqueKids.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Repository
{
    public class CartDetailsRepository : ICartDetails
    {
        private ApplicationDbContext _context;

        public CartDetailsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public CartDetails Add(CartDetails model)
        {
            _context.CartDetails.Add(model);
            _context.SaveChanges();
            return model;
        }

        public CartDetails DeleteCartDetail(int id)
        {
            CartDetails deletedCartDetails=_context.CartDetails.Find(id);
            if (deletedCartDetails != null)
            {
                _context.CartDetails.Remove(deletedCartDetails);
                _context.SaveChanges();
            }
            return deletedCartDetails;
        }

        public CartDetails GetCartDetail(int id)
        {
            return _context.CartDetails.Find(id);
        }

        public List<CartDetails> GetCartDetails() => _context.CartDetails.ToList();

        public CartDetails UpdateCartDetail(CartDetails model)
        {
            var cartDetail = _context.CartDetails.Find(model.Id);
            _context.CartDetails.Update(cartDetail);
            _context.SaveChanges();
            return cartDetail;
        }
    }
}

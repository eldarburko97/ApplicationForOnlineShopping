using AppBoutiqueKids.Data;
using AppBoutiqueKids.Models;
using AppBoutiqueKids.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Repository
{
    public class OrderDetailsRepository : IOrderDetails
    {
        private ApplicationDbContext _context;

        public OrderDetailsRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public OrderDetails Add(OrderDetails model)
        {
            _context.OrderDetails.Add(model);
            _context.SaveChanges();
            return model;
        }

        public OrderDetails DeleteOrderDetail(int id)
        {
            var deletedOrderDetails=_context.OrderDetails.Find(id);
            _context.OrderDetails.Remove(deletedOrderDetails);
            _context.SaveChanges();
            return deletedOrderDetails;
        }

        public OrderDetails GetOrderDetail(int id)
        {
            return _context.OrderDetails.Find(id);
        }

        public List<OrderDetails> GetOrderDetails()
        {
            return _context.OrderDetails.ToList();
        }
    }
}

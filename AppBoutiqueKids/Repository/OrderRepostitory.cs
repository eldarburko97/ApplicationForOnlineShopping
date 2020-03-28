using AppBoutiqueKids.Data;
using AppBoutiqueKids.Models;
using AppBoutiqueKids.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Repository
{
    public class OrderRepostitory : IOrder
    {
        private ApplicationDbContext _context;

        public OrderRepostitory(ApplicationDbContext context)
        {
            _context = context;
        }
        public Order Add(Order model)
        {
            _context.Orders.Add(model);
            _context.SaveChanges();
            return model;
        }

        public Order DeleteOrder(int id)
        {
            var deletedOrder = _context.Orders.Find(id);
            _context.Orders.Remove(deletedOrder);
            _context.SaveChanges();
            return deletedOrder;
        }

        public Order GetOrder(int id)
        {
            return _context.Orders.Find(id);
        }

        public List<Order> GetOrders() => _context.Orders.ToList();
        
    }
}

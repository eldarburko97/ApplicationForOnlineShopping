using AppBoutiqueKids.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Services
{
    public interface IOrder
    {
        Order Add(Order model);
        Order GetOrder(int id);
        Order DeleteOrder(int id);
        List<Order> GetOrders();
    }
}

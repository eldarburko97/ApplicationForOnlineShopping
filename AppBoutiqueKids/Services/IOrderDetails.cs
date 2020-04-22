using AppBoutiqueKids.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Services
{
    public interface IOrderDetails
    {
        OrderDetails Add(OrderDetails model);
        OrderDetails GetOrderDetail(int id);
        OrderDetails DeleteOrderDetail(int id);
        List<OrderDetails> GetOrderDetails();
    }
}

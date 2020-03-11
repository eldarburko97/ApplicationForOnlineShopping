using AppBoutiqueKids.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Services
{
    public interface IShipper
    {
        Shipper AddShipper(Shipper shipper);
        Shipper DeleteShipper(int id);
        Shipper UpdateShipper(Shipper model);
        Shipper GetShipper(int id);
        IEnumerable<Shipper> GetShippers();
    }
}

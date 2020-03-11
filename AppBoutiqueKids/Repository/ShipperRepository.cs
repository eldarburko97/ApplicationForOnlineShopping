using AppBoutiqueKids.Data;
using AppBoutiqueKids.Models;
using AppBoutiqueKids.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Repository
{
    public class ShipperRepository : IShipper
    {
        private ApplicationDbContext _context;

        public ShipperRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Shipper> GetShippers() => _context.Shippers;
        Shipper IShipper.AddShipper(Shipper shipper)
        {
            _context.Shippers.Add(shipper);
            _context.SaveChanges();
            return shipper;
        }
        Shipper IShipper.DeleteShipper(int id)
        {
            Shipper deleteShipper = _context.Shippers.Find(id);
            if (deleteShipper != null)
            {
                _context.Shippers.Remove(deleteShipper);
                _context.SaveChanges();
            }
            return deleteShipper;
        }
        Shipper IShipper.GetShipper(int id)
        {
            Shipper shipper = _context.Shippers.Find(id);
            return shipper;
        }
        Shipper IShipper.UpdateShipper(Shipper model)
        {
            Shipper updateShipper = _context.Shippers.Find(model.Id);
            if (updateShipper != null)
            {
                updateShipper.Name = model.Name;
                updateShipper.PhoneNumber = model.PhoneNumber;
                _context.Shippers.Update(updateShipper);
                _context.SaveChanges();
            }
            return updateShipper;
        }
    }
}

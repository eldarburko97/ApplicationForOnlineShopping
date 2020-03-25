using AppBoutiqueKids.Data;
using AppBoutiqueKids.Models;
using AppBoutiqueKids.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Repository
{
    public class SizeRepository:ISize
    {
        private ApplicationDbContext _context;

        public SizeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        Size ISize.GetSize(int id)
        {
            Size size = _context.Size.Find(id);
            return size;
        }
        public List<Size> GetSizes() => _context.Size.ToList();
        Size ISize.AddSize(Size size)
        {
            _context.Size.Add(size);
            _context.SaveChanges();
            return size;
        }
        Size ISize.DeleteSize(int id)
        {
            Size deleteSize = _context.Size.Find(id);
            if (deleteSize != null)
            {
                _context.Size.Remove(deleteSize);
                _context.SaveChanges();
            }
            return deleteSize;
        }
        Size ISize.UpdateSize(Size newSize)
        {
            Size updateSize = _context.Size.Find(newSize.Id);
            

            if (updateSize != null)
            {
                updateSize.Name = newSize.Name;
                _context.Size.Update(updateSize);
                _context.SaveChanges();
            }
            return updateSize;
        }
    }
}

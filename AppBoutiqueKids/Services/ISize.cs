using AppBoutiqueKids.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Services
{
    public interface ISize
    {
         Size AddSize(Size size);
        Size DeleteSize(int id);
        Size UpdateSize(Size model);
        Size GetSize(int id);
        List<Size> GetSizes();
    }
}

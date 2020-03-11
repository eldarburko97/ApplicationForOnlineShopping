using AppBoutiqueKids.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.ViewModels
{
    public class ProductInputVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
       [Display(Name="Product Image")]
       public IFormFile Photo { get; set; }
        //slika


        [Display(Name = "Brand")]
        public List<SelectListItem> Brands { get; set; }
       
        [Display(Name = "Category")]
        public List<SelectListItem> Categories { get; set; }
       
        public string ExistingPhotoPath { get; set; }
    }
}

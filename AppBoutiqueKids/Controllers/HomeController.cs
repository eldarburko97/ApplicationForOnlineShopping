﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AppBoutiqueKids.Models;
using AppBoutiqueKids.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AppBoutiqueKids.Services;
using AppBoutiqueKids.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using System;

namespace AppBoutiqueKids.Controllers
{
    //[Authorize(Roles =Globals.Member)]
    public class HomeController : Controller
    {
        private IProduct _reposProduct;
        private ApplicationDbContext _context;
        private UserManager<User> _userManager;

        public HomeController(ApplicationDbContext context, IProduct reposProduct, UserManager<User> userManager)
        {
            _reposProduct = reposProduct;
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            if (User.Claims.Any(x => x.Type == ClaimTypes.Role && x.Value == "ADMIN"))
            {
                return RedirectToAction("AdminHomePage", "Admin");
            }
            return RedirectToAction("ProductList");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult ContactUs()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Cart(ProductCartViewModel model)
        //{

            

            

        //    // List<CartDetailsViewModel> list = new List<CartDetailsViewModel>();

        //    //var list = _context.CartDetails.Where(w => w.CartId == cart.Id && cart.UserId == model.UserId).Select(s => new CartDetailsViewModel
        //    //{
        //    //    CartDetailsId = s.Id,
        //    //    PhotoPath = s.ProductSize.Product.ProductImagePath,
        //    //    ProductName = s.ProductSize.Product.Name,
        //    //    Quantity = s.Quantity,
        //    //    Price = s.ProductSize.Product.Price
        //    //}).ToList();

        //    return View(list);
        //}

        public IActionResult ManView()
        {
            return View();
        }
        public IActionResult WomanView()
        {
            return View();
        }

        public IActionResult ProductDetails(int id)
        {
            var product = _reposProduct.GetProduct(id);
            var selectLista = _context.ProductSizes.Where(p => p.ProductId == product.Id).Select(s => new SelectListItem
            {
                Value = s.ProductSizeId.ToString(),
                Text = s.Size.Name
            }).ToList();
            ProductCartViewModel model = new ProductCartViewModel
            {
                Id = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                PhotoPath = product.ProductImagePath,
                Brand = product.Brand.Name,
                Category = product.Category.Name,
                UserId = int.Parse(_userManager.GetUserId(HttpContext.User)),
                ProductSizes = selectLista
            };
            return View(nameof(ProductDetails), model);
        }

        public IActionResult ProductList()
        {
            return View(_reposProduct.GetProducts());
        }

    }
}

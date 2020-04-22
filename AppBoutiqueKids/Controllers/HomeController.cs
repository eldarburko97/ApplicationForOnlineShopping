using System.Collections.Generic;
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
using Microsoft.AspNetCore.SignalR;
using AppBoutiqueKids.Hubs;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Controllers
{
    public class HomeController : Controller
    {
        private IProduct _reposProduct;
        private ICartDetails _reposCartDetails;
        private ApplicationDbContext _context;
        private UserManager<User> _userManager;
        private IHubContext<DeliverHub> _hubContext;
        private IHubContext<NotificationsHub> _cartHubContext;
        public HomeController(IHubContext<DeliverHub> hub,
            IHubContext<NotificationsHub> cartHubContext,
            ApplicationDbContext context, 
            IProduct reposProduct, 
            ICartDetails reposCartDetails, 
            UserManager<User> userManager
            )
        {
            _reposProduct = reposProduct;
            _reposCartDetails = reposCartDetails;
            _context = context;
            _userManager = userManager;
            _hubContext = hub;
            _cartHubContext = cartHubContext;
        }

        public IActionResult Index()
        {
            if (User.Claims.Any(x => x.Type == ClaimTypes.Role && x.Value == "ADMIN"))
            {
                return RedirectToAction("AdminHomePage", "Admin");
            }
            return RedirectToAction(nameof(ProductList));
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

        [HttpGet]
        public IActionResult YourCart()
        {
            int userId = int.Parse(_userManager.GetUserId(HttpContext.User));
            var listOfCartDetails = _context.CartDetails.Where(cd => cd.UserId == userId).Select(s => new CartDetailsViewModel
            {
                CartDetailsId = s.Id,
                PhotoPath = s.ProductSize.Product.ProductImagePath,
                ProductName = s.ProductSize.Product.Name,
                Quantity = s.Quantity,
                Price = s.ProductSize.Product.Price
            }).ToList();
            ViewBag.UserId = userId;
            return View(listOfCartDetails);
        }

        [HttpPost]
        public IActionResult Cart(ProductCartViewModel model)
        {
            CartDetails newCartDetail = new CartDetails
            {
                UserId = model.UserId,
                Quantity = model.QuantityForBuy,
                ProductSizeId = model.ProductSizeId
            };
            _reposCartDetails.Add(newCartDetail);
             
            var listOfCartDetails = _context.CartDetails.Where(cd => cd.UserId == model.UserId).Select(s => new CartDetailsViewModel
            {
                CartDetailsId = s.Id,
                PhotoPath = s.ProductSize.Product.ProductImagePath,
                ProductName = s.ProductSize.Product.Name,
                Quantity = s.Quantity,
                Price = s.ProductSize.Product.Price
            }).ToList();

            ViewData["userId"] = newCartDetail.UserId;
            return View(listOfCartDetails);
        }

        [HttpPost]
        public IActionResult AddToCart(ProductCartViewModel model)
        {
            CartDetails newCartDetail = new CartDetails
            {
                UserId = model.UserId,
                Quantity = model.QuantityForBuy,
                ProductSizeId = model.ProductSizeId
            };
            _reposCartDetails.Add(newCartDetail);

            return Ok();
        }

        [HttpGet]
        public IActionResult GetCartCount(int Id)
        {
            // get total cart count for userId in CartService

            int count = _context.CartDetails.Where(w => w.UserId == Id).Count();

            return Ok(count);
        }

        public IActionResult DeleteCartDetails(int id)
        {
            var cartDetail = _reposCartDetails.GetCartDetail(id);
            var userId = cartDetail.UserId;
            ViewData["userId"] = userId;
            _reposCartDetails.DeleteCartDetail(id);
            return View(nameof(Cart), _context.CartDetails.Where(cd => cd.UserId == userId).Select(s => new CartDetailsViewModel
            {
                CartDetailsId = s.Id,
                PhotoPath = s.ProductSize.Product.ProductImagePath,
                ProductName = s.ProductSize.Product.Name,
                Quantity = s.Quantity,
                Price = s.ProductSize.Product.Price
                
            }).ToList());
        }


        [HttpPost]
        public async Task<IActionResult> ConfirmOrder(int UserId)
        {
            Order order = new Order
            {
                OrderDate = DateTime.Now,
                UserId = UserId
            };

            _context.Orders.Add(order);
            _context.SaveChanges();
            var listofCartDetails = _context.CartDetails.Where(cd => cd.UserId == order.UserId).ToList();
            foreach (var d in listofCartDetails)
            {
                OrderDetails orderDetails = new OrderDetails
                {
                    OrderId = order.Id,
                    ProductSizeId = d.ProductSizeId
                };
                _context.OrderDetails.Add(orderDetails);
                _context.SaveChanges();
                var productId = _context.ProductSizes.Find(d.ProductSizeId);
                var product = _context.Products.Find(productId.ProductId);
                product.Quantity -= d.Quantity;
                _context.Products.Update(product);
                _context.SaveChanges();

                await _hubContext.Clients.All.SendAsync("RecieveUpdatedQuantity", product.Quantity);
            }
            foreach (var ld in listofCartDetails)
            {
                _reposCartDetails.DeleteCartDetail(ld.Id);
            }


            return RedirectToAction(nameof(ProductList));
        }

        public IActionResult WomansView()
        {
            var listOfWomenProducts = _context.Products.Include(b => b.Brand).Include(c => c.Category).Where(p => p.Category.Name == "Female").ToList();
            return View(listOfWomenProducts);
        }
        public IActionResult MansView()
        {
            var listOfMenProducts = _context.Products.Include(b => b.Brand).Include(c => c.Category).Where(p => p.Category.Name == "Male").ToList();
            return View(listOfMenProducts);
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

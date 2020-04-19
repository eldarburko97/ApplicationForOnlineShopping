using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AppBoutiqueKids.Data;
using AppBoutiqueKids.Hubs;
using AppBoutiqueKids.Models;
using AppBoutiqueKids.Services;
using AppBoutiqueKids.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace AppBoutiqueKids.Controllers
{
    [Authorize(Roles = Globals.Admin)]
    public class AdminController : Controller
    {
        private ISize _reposSize;
        private IBrand _reposBrand;
        private IShipper _reposShipper;
        private IProduct _reposProduct;
        private ICategory _reposCategory;
        private ISupplier _reposSupplier;
        private IProductSize _reposProductSize;
        private IOrder _reposOrder;
        private ApplicationDbContext _context;
        private IHubContext<NotificationsHub> _hubContext;
        private readonly IUserData userData;
        private readonly UserManager<User> userManager;
        private readonly IHostingEnvironment _hostingEnvironment;

        public AdminController(
            IUserData userData,
            UserManager<User> userManager,IHostingEnvironment hostingEnvironment,
            ISize reposSize,IBrand reposBrand,
            IShipper reposShipper, IProduct reposProduct,
            ICategory reposCategory,ISupplier reposSupplier,IProductSize reposProductSize,
            IOrder reposOrder,ApplicationDbContext context, Microsoft.AspNetCore.SignalR.IHubContext<NotificationsHub> hubContext)
        {
            this.userData = userData;
            this.userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
            _reposSize = reposSize;
            _reposBrand = reposBrand;
            _reposShipper = reposShipper;
            _reposProduct = reposProduct;
            _reposCategory = reposCategory;
            _reposSupplier = reposSupplier;
            _reposProductSize = reposProductSize;
            _reposOrder = reposOrder;
            _context = context;
            _hubContext = hubContext;
        }

        [Authorize(Roles = Globals.Admin)]
        public IActionResult AdminHomePage() //Dashboard
        {
            return View();
        }

        //[HttpPost]
        public async Task<IActionResult> AddToRole(int id) // UserID 
        {
            User user = userData.Get(id);
            if (await userManager.IsInRoleAsync(user, Globals.Admin))
            {
                return RedirectToAction(nameof(AdminHomePage));
            }
            userData.AddToRole(id, Globals.Admin);
            return RedirectToAction(nameof(Index), "Home");
        }


        public IActionResult AddProduct()
        {
            ProductInputVM vm = new ProductInputVM();
           // List<Brand> brands = new List<Brand>();
            // List<Category> categories = new List<Category>();

            // brands = _context.Brands.ToList();
            // categories = _context.Categories.ToList();
           var brands = _reposBrand.GetBrands();
           var categories = _reposCategory.GetCategories();

            vm.Brands = brands.Select(b => new SelectListItem(b.Name, b.Id.ToString())).ToList();
            vm.Categories = categories.Select(c => new SelectListItem(c.Name, c.Id.ToString())).ToList();
            return View(vm);
        }
        
        [HttpPost]
        public IActionResult AddProduct(ProductInputVM vm)
        {

            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (vm.Photo != null)
                {
                    string UploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Path.GetFileName(vm.Photo.FileName);
                    string filePath = Path.Combine(UploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        vm.Photo.CopyTo(fileStream);
                    }
               
                }
                Product product = new Product
                {
                    Name = vm.Name,
                    Price = vm.Price,
                    Quantity = vm.Quantity,
                    BrandId = vm.BrandId,
                    CategoryId = vm.CategoryId,
                    ProductImagePath = uniqueFileName
                };
                _reposProduct.AddProduct(product);
                var listOfSize = _reposSize.GetSizes();
                for(var s=0;s<listOfSize.Count();s++)
                {
                    ProductSize productSize = new ProductSize
                    {
                        ProductId = product.Id,
                        SizeId = listOfSize[s].Id
                    };
                    _reposProductSize.Add(productSize);
                }
                return RedirectToAction("ProductList","Home");
            }
            return RedirectToAction("AddProduct");
        }

        public IActionResult DeleteProduct(int Id)
        {
          //  Product product = _context.Products.FirstOrDefault(p => p.Id == Id);
            _reposProduct.DeleteProduct(Id);
          //  _context.Products.Remove(product);
          //  _context.SaveChanges();

            return RedirectToAction("ProductList", "Home");
        }

        public IActionResult UpdateProduct(int Id)
        {
            ProductInputVM vm = new ProductInputVM();
            //List<Brand> brands = new List<Brand>();
            // List<Category> categories = new List<Category>();
            //Product product = new Product();
            var product = _reposProduct.GetProduct(Id);

            var brands=_reposBrand.GetBrands();
            //  categories = _context.Categories.ToList();
           var categories = _reposCategory.GetCategories();
            vm.Id = product.Id;
            vm.Name = product.Name;
            vm.Price = product.Price;
            vm.Quantity = product.Quantity;
            vm.BrandId = product.BrandId;
            vm.CategoryId = product.CategoryId;
            vm.ExistingPhotoPath = product.ProductImagePath;
            vm.Brands = brands.Select(b => new SelectListItem(b.Name, b.Id.ToString())).ToList();
            vm.Categories = categories.Select(c=>new SelectListItem(c.Name,c.Id.ToString())).ToList();
                  
            return View(vm);
        }

        [HttpPost]
        public IActionResult UpdateProduct(ProductInputVM vm)
        {
            if (ModelState.IsValid)
            {
                //  Product product = _context.Products.FirstOrDefault(p => p.Id == vm.Id);
                Product product = _reposProduct.GetProduct(vm.Id);
                product.Name = vm.Name;
                product.Price = vm.Price;
                product.Quantity = vm.Quantity;
                product.BrandId = vm.BrandId;
                product.CategoryId = vm.CategoryId;

                string FileName = null;

                if (vm.Photo != null)
                {
                    if (vm.ExistingPhotoPath != null)
                    {
                        string filePathDel = Path.Combine(_hostingEnvironment.WebRootPath, "images", vm.ExistingPhotoPath);
                        System.IO.File.Delete(filePathDel);
                    }   
                    
                    string UploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                    FileName = Path.GetFileName(vm.Photo.FileName);
                    string filePath = Path.Combine(UploadsFolder, FileName);
                    using(var fileStream= new FileStream(filePath, FileMode.Create))
                    {
                        vm.Photo.CopyTo(fileStream);
                    }
                 

                    product.ProductImagePath = FileName;
                }

                if(vm.Photo==null)
                {
                    if (vm.ExistingPhotoPath != null)
                    {
                        product.ProductImagePath = vm.ExistingPhotoPath;
                    }
                    else
                        product.ProductImagePath = null;
                }

                //if(vm.Photo==null)
                // {
                //     if(vm.ExistingPhotoPath==null)
                //     {
                //         product.ProductImagePath = null;
                //     }
                // }


                //  _context.Products.Update(product);
                //  _context.SaveChanges();
                _reposProduct.UpdateProduct(product);
                return RedirectToAction("ProductList","Home");
            }
            return RedirectToAction("AddProduct");

        }

       
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCategory(CategoryInputViewModel model)
        {
            if (ModelState.IsValid)
            {
                Category c = new Category
                {
                    Name = model.Name
                };
             
                _reposCategory.AddCategory(c);

                return RedirectToAction("CategoryList");
            }
            return RedirectToAction("AddCategory");
        }

        public IActionResult CategoryList()
        {
            return View(_reposCategory.GetCategories());
        }

        public IActionResult DeleteCategory(int Id)
        {
            _reposCategory.DeleteCategory(Id);
            return RedirectToAction("CategoryList");
        }

        public IActionResult UpdateCategory(int Id)
        {          
           var category = _reposCategory.GetCategory(Id);
            return View(category);
        }

        [HttpPost]
        public IActionResult UpdateCategory(Category cat)
        {
            if (ModelState.IsValid)
            {
               var category = _reposCategory.GetCategory(cat.Id);
                category.Name = cat.Name;
                _reposCategory.UpdateCategory(category);
                return RedirectToAction("CategoryList");
            }
            return RedirectToAction("UpdateCategory");
        }
        [HttpGet]
        public IActionResult AddSupplier()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddSupplier(Supplier s)
        {
            Supplier supplier = new Supplier()
            {
                Name = s.Name,
                PhoneNumber = s.PhoneNumber
            };
            _reposSupplier.AddSupplier(supplier);
            return RedirectToAction("AdminHomePage", "Admin");
        }

        public IActionResult SuppliersList()
        {
            return View(_reposSupplier.GetSuppliers());
        }
        public IActionResult DeleteSupplier(int id)
        {
            _reposSupplier.DeleteSupplier(id);
            return RedirectToAction("SuppliersList");
        }

        [HttpGet]
        public IActionResult UpdateSupplier(int id)
        {
            var supplier = _reposSupplier.GetSupplier(id);
            return View(supplier);
        }
        [HttpPost]
        public IActionResult UpdateSupplier(Supplier s)
        {
            var updatedSupplier = _reposSupplier.GetSupplier(s.Id);
            if (updatedSupplier != null)
            {
                updatedSupplier.Name = s.Name;
                updatedSupplier.PhoneNumber = s.PhoneNumber;
                _reposSupplier.UpdateSupplier(updatedSupplier);
            }
            return RedirectToAction("SuppliersList");
        }







        //Ahmed
        public IActionResult AddBrand()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBrand(BrandInputViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.Photo != null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath,
                        "images");
                    uniqueFileName = Guid.NewGuid().ToString() + '_' + model.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                Brand newBrand = new Brand
                {
                    Name = model.Name,
                    Photo = uniqueFileName
                };
                _reposBrand.AddBrand(newBrand);
                return RedirectToAction(nameof(BrandList));
            }
            return RedirectToAction(nameof(AdminHomePage));
        }
        public IActionResult BrandList()
        {
            return View(_reposBrand.GetBrands());
        }


        public IActionResult DeleteBrand(int id)
        {
            _reposBrand.DeleteBrand(id);
            return RedirectToAction(nameof(BrandList));
        }


        public IActionResult UpdateBrand(int id)
        {
            var brand = _reposBrand.GetBrand(id);
            return View(brand);
        }

        [HttpPost]
        public IActionResult UpdateBrand(Brand model)
        {
            _reposBrand.UpdateBrand(model);
            return RedirectToAction(nameof(BrandList));
        }

       
        public IActionResult AddSize()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddSize(Size model)
        {
            _reposSize.AddSize(model);
            return RedirectToAction(nameof(SizeList));
        }
        public IActionResult SizeList()
        {
            return View(_reposSize.GetSizes());
        }
        
        public IActionResult DeleteSize(int id)
        {
            _reposSize.DeleteSize(id);
            return RedirectToAction(nameof(SizeList));
        }

        
        public IActionResult UpdateSize(int id)
        {
            Size updatedSize = _reposSize.GetSize(id);
            return View(updatedSize);
        }
        [HttpPost]
        public IActionResult UpdateSize(Size model)
        {
            _reposSize.UpdateSize(model);
            return RedirectToAction(nameof(SizeList));
        }
        
        public IActionResult AddShipper()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddShipper(Shipper model)
        {
            _reposShipper.AddShipper(model);
            return RedirectToAction(nameof(ShippersList));
        }

        public IActionResult ShippersList()
        {
            return View(_reposShipper.GetShippers());
        }
        public IActionResult DeleteShipper(int id)
        {
            _reposShipper.DeleteShipper(id);
            return RedirectToAction(nameof(ShippersList));
        }
       
        public IActionResult UpdateShipper(int id)
        {
            Shipper updatedShipper = _reposShipper.GetShipper(id);
            return View(updatedShipper);
        }
        [HttpPost]
        public IActionResult UpdateShipper(Shipper model)
        {
            _reposShipper.UpdateShipper(model);
            return RedirectToAction(nameof(ShippersList));
        }
        public IActionResult OrderList()
        {
            var listOfOrders = _context.OrderDetails.Select(o => new OrderListViewModel{
                Id=o.Id,
                Product=o.ProductSize.Product.Name,
                Size=o.ProductSize.Size.Name,
                User=o.Order.User.UserName,
                Email=o.Order.User.Email}).ToList();
            return View(listOfOrders);
        }
        public async Task< IActionResult> DeleteOrder(int id)
        {
            var deleteOrder = _context.OrderDetails.Find(id);
            _context.OrderDetails.Remove(deleteOrder);
            _context.SaveChanges();
            var order = _context.Orders.Find(deleteOrder.OrderId);

            //var user = _context.Users.Find(order.UserId);

            //await _hubContext.Clients.User(user.Id.ToString()).SendAsync("ReceiveMessage", "Vaša narudžba je isporučena!!");
            
            return RedirectToAction(nameof(OrderList));
        }
    }
}
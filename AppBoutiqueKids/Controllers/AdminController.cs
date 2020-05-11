using System;
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
using X.PagedList;


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
        private IOrderDetails _reposOrderDetails;
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
            ICategory reposCategory,ISupplier reposSupplier,IProductSize reposProductSize,IOrderDetails reposOrderDetails,
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
            _reposOrderDetails = reposOrderDetails;
            _context = context;
            _hubContext = hubContext;
        }

        [Authorize(Roles = Globals.Admin)]
        public IActionResult AdminHomePage() 
        {
            return View();
        }

        
        public async Task<IActionResult> AddToRole(int id) 
        {
            User user = userData.Get(id);
            if (await userManager.IsInRoleAsync(user, Globals.Admin))
            {
                return RedirectToAction(nameof(AdminHomePage));
            }
            userData.AddToRole(id, Globals.Admin);
            return RedirectToAction(nameof(Index), "Home");
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            ProductInputVM vm = new ProductInputVM();
          
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
                    BrandId = vm.BrandId.Value,
                    CategoryId = vm.CategoryId.Value,
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
            return View(vm);
            //return RedirectToAction("AddProduct", "Admin");
        }

        public IActionResult DeleteProduct(int Id)
        {
            _reposProduct.DeleteProduct(Id);
            return RedirectToAction("ProductList", "Home");
        }

        public IActionResult UpdateProduct(int Id)
        {
            ProductInputVM vm = new ProductInputVM();
            
            var product = _reposProduct.GetProduct(Id);

            var brands=_reposBrand.GetBrands();
            
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
                
                Product product = _reposProduct.GetProduct(vm.Id);
                product.Name = vm.Name;
                product.Price = vm.Price;
                product.Quantity = vm.Quantity;
                product.BrandId = vm.BrandId.Value;
                product.CategoryId = vm.CategoryId.Value;

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

                
                _reposProduct.UpdateProduct(product);
                return RedirectToAction("ProductList","Home");
            }
            return RedirectToAction("AddProduct");

        }

       [HttpGet]
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
            return View();
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
        public IActionResult AddSupplier(SupplierInputVM model)
        {
            if(ModelState.IsValid)
            {
                Supplier supplier = new Supplier
                {
                    Name = model.Name,
                    PhoneNumber = model.PhoneNumber
                };
                _reposSupplier.AddSupplier(supplier);
                return RedirectToAction("SuppliersList");
            }
            return View();
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
            return RedirectToAction(nameof(AddBrand));
        }
        public IActionResult BrandList()
        {
            var model = _reposBrand.GetBrands();
            return View(model);
        }


        public IActionResult DeleteBrand(int id)
        {
            _reposBrand.DeleteBrand(id);
            return RedirectToAction(nameof(BrandList));
        }


        public IActionResult UpdateBrand(int id)
        {
            var brand = _reposBrand.GetBrand(id);
            BrandInputViewModel model = new BrandInputViewModel
            {
                Id = brand.Id,
                Name = brand.Name,
                PhotoPath = brand.Photo
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateBrand(BrandInputViewModel model)
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
                Brand updateBrand = _reposBrand.GetBrand(model.Id);
                updateBrand.Name = model.Name;
                updateBrand.Photo = uniqueFileName;
                _reposBrand.UpdateBrand(updateBrand);
                return RedirectToAction(nameof(BrandList));
            }
            return View(nameof(UpdateBrand));
        }

       
        public IActionResult AddSize()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddSize(SizeInputVM model)
        {
            if (ModelState.IsValid)
            {
                Size newSize = new Size
                {
                    Name = model.Name
                };
                _reposSize.AddSize(newSize);
                return RedirectToAction(nameof(SizeList));
            }
            return RedirectToAction(nameof(AddSize));
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
        public IActionResult AddShipper(ShipperInputVM model)
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
                Shipper newShipper = new Shipper
                {
                    Name = model.Name,
                    PhoneNumber = model.PhoneNumber,
                    Photo=uniqueFileName
                };
                _reposShipper.AddShipper(newShipper);
                return RedirectToAction(nameof(ShippersList));
            }
            return View(nameof(AddShipper));
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
        public IActionResult OrderList(int? pageNumber=null)
        {
            
            var listOfOrders = _context.OrderDetails.Select(o => new OrderListViewModel {
                Id = o.Id,
                Product = o.ProductSize.Product.Name,
                Size = o.ProductSize.Size.Name,
                User = o.Order.User.UserName,
                Email = o.Order.User.Email,
                FirstLastName=o.Order.User.FirstName+' '+o.Order.User.LastName,
                CityAdressZipCode=o.Order.User.City+' '+o.Order.User.ZipCode+','+o.Order.User.Adress,
                Phone=o.Order.User.PhoneNumber
            }).ToList();

            IPagedList<OrderListViewModel> list = listOfOrders.ToPagedList(pageNumber ?? 1, 5);
            
            return View(list);
        }
        public IActionResult DeleteOrder(int id)
        {
            _reposOrderDetails.DeleteOrderDetail(id);
            return RedirectToAction(nameof(OrderList));
        }
    }
}
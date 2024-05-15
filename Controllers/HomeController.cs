using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Identity.Models;
using MVC.Identity.Services.Interfaces;
using MVC.Identity.ViewModels.Product;
using System.Diagnostics;

namespace MVC.Identity.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;

        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var products = await _productService.GetAllProducts();
            return View(products);
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct(CreateProductVM createProductVM)
        {
            var user = User.Identity.Name;
            if (ModelState.IsValid)
            {
                Product product = new()
                {
                    Name = createProductVM.Name,
                    Description = createProductVM.Description,
                    Price = createProductVM.Price,
                    CreatedBy = user,
                };
                await _productService.CreateProduct(product);
                TempData["Success"] = "Product created successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(createProductVM);
        }

        [HttpGet]
        [Authorize(Roles ="Super Admin, Admin")]
        public async Task<ActionResult> EditProduct(int id)
        {
            Product product = await _productService.GetProductById(id);

            if (product != null)
            {
                EditProductVM editProductVM = new()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price
                };
                return View(editProductVM);
            }
            return View("NotFound");
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin, Admin")]
        public async Task<ActionResult> EditProduct(EditProductVM editProductVM)
        {
            if (ModelState.IsValid)
            {
                Product prod = await _productService.GetProductById(editProductVM.Id);
                prod.Name = editProductVM.Name;
                prod.Description = editProductVM.Description;
                prod.Price = editProductVM.Price;

                await _productService.UpdateProduct(prod);
                TempData["Message"] = "Product updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(editProductVM);
        }

        [Authorize(Roles ="Super Admin")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var productToDelete = await _productService.GetProductById(id);
            if (productToDelete != null)
            {
                await _productService.DeleteProduct(id);
                TempData["Message"] = "Product deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View("NotFound");
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
    }
}

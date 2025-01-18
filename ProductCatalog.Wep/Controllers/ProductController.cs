using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Application.DTOs;
using ProductCatalog.Application.Interfaces;
using ProductCatalog.Application.Services;
using ProductCatalog.Core.Entities;
using System.IO;

namespace ProductCatalog.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(
            IProductService productService,
            UserManager<ApplicationUser> userManager,
            ICategoryService categoryService,
            ILogger<ProductsController> logger)
        {
            _productService = productService;
            _userManager = userManager;
            _categoryService = categoryService;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(int? categoryId)
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name", categoryId);
                IEnumerable<ProductDto> products;
               
                products = await _productService.GetProductsAsync(categoryId);
                ViewBag.IsOnTime = false;
                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products for category ID: {CategoryId}", categoryId);
                return View("Error", new { Message = "Failed to load products." });
            }
        }

        [Authorize]
        public async Task<IActionResult> IndexOnTime(int? categoryId)
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name", categoryId);
                var products = await _productService.GetActiveProductsByCategoryAsync(categoryId);
                ViewBag.IsOnTime = true;
                return View("Index", products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching on-time products for category ID: {CategoryId}", categoryId);
                return View("Error", new { Message = "Failed to load on-time products." });
            }
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product details for ID: {Id}", id);
                return View("Error", new { Message = "Failed to load product details." });
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateOrEdit(int? id)
        {
            try
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");

                if (id == null || id == 0)
                {
                    return View(new ProductDto());
                }
                else
                {
                    var product = await _productService.GetProductByIdAsync(id.Value);
                    if (product == null)
                    {
                        return NotFound();
                    }
                    return View(product);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading create/edit view for product ID: {Id}", id);
                return View("Error", new { Message = "Failed to load create/edit view." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(int? id, ProductDto productDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(await _categoryService.GetAllCategoriesAsync(), "Id", "Name");
                return View(productDto);
            }

            try
            {
                var userId = _userManager.GetUserId(User);
                productDto.CreatedByUserId = userId;

                if (productDto.ImageFile != null)
                {
                  productDto.ImagePath=await _productService.HandleImageUploadAsync(productDto.ImageFile);
                 
                }

                if (id == null || id == 0)
                {
                    await _productService.AddProductAsync(productDto);
                }
                else
                {
                    productDto.Id = id.Value;
                    await _productService.UpdateProductAsync(productDto);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                _logger.LogError(ex, "Error creating/updating product ID: {Id}", id);
                return View("Error", new { Message = "Failed to save product." });
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                return View(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading delete view for product ID: {Id}", id);
                return View("Error", new { Message = "Failed to load delete view." });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product ID: {Id}", id);
                return View("Error", new { Message = "Failed to delete product." });
            }
        }

     

    }
}

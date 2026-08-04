using FirstMVCProject.Data;
using FirstMVCProject.Dto;
using FirstMVCProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstMVCProject.Controllers
{
    public class DashBoardController(AppDbContext context) : Controller
    {
        public IActionResult Index()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            var Products = context.Products.Select(x => new ProductDto { Id = x.Id, ProductName = x.ProductName, Description = x.Description, Price = x.Price, Color = x.Color }).ToList();

            return View(Products);
        }

        public IActionResult ProductForm()
        {
            return View();
        }
        public IActionResult EditForm(int productid)
        {
            var product = context.Products.Select(p => new ProductDto
            {
                Id = p.Id,
                ProductName = p.ProductName,
                Description = p.Description,
                Price = p.Price,
                Color = p.Color
            }).FirstOrDefault(p => p.Id == productid);

            if (product == null)
            {
                ViewBag.ErrorMessage = "Product not found.";
                return RedirectToAction("Index");
            }

            

            return View(product);
        }


        public async Task<IActionResult> AddProduct(ProductDto dto)
        {
            Console.WriteLine("I am inside the Add Product method");
            if (dto == null)
            {
                ViewBag.ErrorMessage = "Invalid product data.";

                return View("ProductForm");
            }

            var existingProduct = await context.Products.FirstOrDefaultAsync(p => p.ProductName == dto.ProductName);

            if (existingProduct != null)
            {
                ViewBag.ErrorMessage = "Product with the same name already exists.";
                return View("ProductForm");
            }
            else
            {
                var Product = new Product
                {
                    Id = dto.Id,
                    ProductName = dto.ProductName,
                    Description = dto.Description,
                    Price = dto.Price,
                    Color = dto.Color
                };

                context.Products.Add(Product);
                await context.SaveChangesAsync();

            }


            ViewBag.SuccessMessage = "Product added successfully.";
            return RedirectToAction("Index");


        }


        public async Task<IActionResult> DeleteProduct(int productid)
        {
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == productid);
            Console.WriteLine(product);
            if (product == null)
            {
                ViewBag.ErrorMessage = "Product not found.";
                return RedirectToAction("Index");
            }
            else
            {
                context.Products.Remove(product);
                await context.SaveChangesAsync();

            }

            ViewBag.SuccessMessage = "Product deleted successfully.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> EditProduct(ProductDto dto)
        {
            if (dto == null)
            {
                ViewBag.ErrorMessage = "Invalid product data.";
                return View("EditForm");
            }
            else
            {
                var product = await context.Products.FirstOrDefaultAsync(p => p.Id == dto.Id);
                if (product == null)
                {
                    ViewBag.ErrorMessage = "Product not found.";
                    return RedirectToAction("Index");
                }
                else
                {
                    product.ProductName = dto.ProductName;
                    product.Description = dto.Description;
                    product.Price = dto.Price;
                    product.Color = dto.Color;

                    context.Products.Update(product);
                    await context.SaveChangesAsync();
                }
                
            }

            TempData["SuccessMessage"] = "Product updated successfully.";

            return RedirectToAction("Index"); return RedirectToAction("Index");
        }

    }
}

using FirstMVCProject.Data;
using FirstMVCProject.Dto;
using FirstMVCProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstMVCProject.Controllers
{
    public class AuthController(AppDbContext context) : Controller()
    {
        public IActionResult Login()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public async Task<IActionResult> CreateUser(UserDto dto)
         {
            if (dto==null || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password) || string.IsNullOrEmpty(dto.Username))
            {
                ViewBag.ErrorMessage = "Please fill in all required fields";
                return View("Register");
            }
            var ExistingUser = await context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (ExistingUser == null)
            {

                var user = new User
                {
                    Email = dto.Email,
                    Password = dto.Password,
                    Username = dto.Username
                };

                context.Users.Add(user);
                await context.SaveChangesAsync();
            }
            else
            {
                ViewBag.ErrorMessage = "User with this email already exist";
                return View("Register");
            }

            TempData["SuccessMessage"] = "User created successfully";

            return RedirectToAction("Login");



        }
    }
}

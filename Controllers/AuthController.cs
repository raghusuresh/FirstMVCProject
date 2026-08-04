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
            if (dto == null || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password) || string.IsNullOrEmpty(dto.Username))
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

        public async Task<IActionResult> LoginUser(UserDto dto)
        {

            if (dto == null || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password))
            {
                ViewBag.ErrorMessage("Kindly fill all the Details.");
                return View("Login");
            }
            var existinguser = await context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (existinguser == null)
            { 

                ViewBag.ErrorMessage = "The user Email does not exist";
                return View("Login");
            }
            else if (existinguser.Password != dto.Password)
            {
                ViewBag.ErrorMessage = "The password is incorrect";
                return View("Login");
            }
            else 
            {
                TempData["SuccessMessage"] = "User logged in successfully";
                return RedirectToAction("Index", "DashBoard");
            }

        }

    }



        
}

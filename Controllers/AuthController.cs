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
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public async Task<IActionResult> CreateUser(UserDto dto)
         {
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
                ModelState.AddModelError("Email", "Email already exists");
                Console.WriteLine("Email already exists");
            }
            return RedirectToAction("Login");



        }
    }
}

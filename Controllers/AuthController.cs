using FirstMVCProject.Data;
using FirstMVCProject.Dto;
using FirstMVCProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
                var token = GenerateJWTToken(dto);

                Response.Cookies.Append("jwt_key", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(30)
                });

                TempData["SuccessMessage"] = "User logged in successfully";
                return RedirectToAction("Index", "DashBoard");
            }

        }

        private string GenerateJWTToken(UserDto dto)
        {
            var jwthandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("jY83FQqjIyjdfyyLcMGjQsCYIIjxNaMsHzoVz0auFyp");

            var TokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, dto.Email), }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(new SymmetricSecurityKey(key), Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
            };
            var token = jwthandler.CreateToken(TokenDescriptor);
            return jwthandler.WriteToken(token);
        }



        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt_key");
            TempData["SuccessMessage"] = "User logged out successfully";
            return RedirectToAction("Login");

        }

    }



    }

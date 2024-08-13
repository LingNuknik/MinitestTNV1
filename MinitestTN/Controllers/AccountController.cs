using Course.Core.Common;
using DataAccess.Da;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MinitestTN.Models;
using System.Data;

namespace MinitestTN.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserDa userDa;
        private readonly IConfiguration configuration;
        private readonly string UserLogin;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserDa userDa, IConfiguration configuration)
        {
            this.signInManager = signInManager;
            this.userDa = userDa;
            this.configuration = configuration;
            UserLogin = configuration.GetValue<String>("UserLogin");
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }


        //[Authorize]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationModel data)
        {
            var a = new ApplicationUser
            {
                FirstName = data.FirstName,
                LastName = data.LastName,
                UserName = data.Username,
                Password = data.Password,
                CreateBy = User.Identity.Name,
                CreateDate = DateTime.Now,
                IsActive = true,
                PhoneNumber = data.PhoneNumber,
                Email = data.Email,
                Role = Role.System
            };

            await userDa.Insert(a);

            return RedirectToAction("Login", "Account");
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel data)
        {
            var result = await signInManager.PasswordSignInAsync(data.Username, data.Password, data.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Furniture");
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();


            return RedirectToAction("Login", "Account");
        }
    }
}

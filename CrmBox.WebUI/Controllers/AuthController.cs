using CrmBox.Core.Domain.Identity;
using CrmBox.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CrmBox.WebUI.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        readonly SignInManager<AppUser> _signInManager;
        readonly UserManager<AppUser> _userManager;
        readonly RoleManager<AppRole> _roleManager;

        public AuthController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        [HttpGet]
        public async Task<IActionResult> Login()
        {
            var roots = await _userManager.GetUsersInRoleAsync("root");
            if (roots.Count == 0)
            {
                AppUser rootUser = new() { FirstName = "root", LastName = "root", UserName = "root" };
                IdentityResult result = await _userManager.CreateAsync(rootUser, "pswrd");
                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(rootUser, "ROOT");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserForLoginVM vM)
        {

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(vM.Username, vM.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("GetAllCustomers", "Customers");
                }
                else
                    return View();

            }
            return View();

        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Auth");
        }

    }
}

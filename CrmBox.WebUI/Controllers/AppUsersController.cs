using CrmBox.Application.Interfaces.Customer;
using CrmBox.Application.Services.Customer;
using CrmBox.Core.Domain;
using CrmBox.Core.Domain.Identity;
using CrmBox.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using System.Data;

namespace CrmBox.WebUI.Controllers
{
    [Authorize(Roles = "Admin,Moderator")]
    public class AppUsersController : Controller
    {

        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        IMemoryCache _memoryCache;
        const string cacheKey = "customerKey";
        public AppUsersController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IMemoryCache memoryCache)

        {
            _userManager = userManager;

            _signInManager = signInManager;
            _memoryCache = memoryCache;
        }

        [HttpGet]

        public IActionResult GetAllUsers()
        {
            var users = _userManager.Users.ToList();

            if (!_memoryCache.TryGetValue(cacheKey, out object list))
            {

                var cacheExpOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(1),
                    Priority = CacheItemPriority.Normal
                };

                _memoryCache.Set(cacheKey, users, cacheExpOptions);
            }

            //var userRole = _roleManager.Roles.Select(x => x.Name).FirstOrDefault();
            //ViewBag.userRole = userRole;
            return View(users);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddUser()
        {
            //AddUserVM appUser = new();

            //var Roles = _roleManager.Roles.Select(x => new RoleWithSelectVM
            //{
            //    Id = x.Id,
            //    Name = x.Name,
            //    IsSelected = false,
            //    Claims = _roleManager.GetClaimsAsync(x).Result
            //}).ToList();

            //appUser.Roles = Roles;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUser(AddUserVM model)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser
                {

                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.UserName,
                    Email = model.Email,
                    Password = model.Password,



                };

                IdentityResult result = await _userManager.CreateAsync(appUser, model.Password);
                if (result.Succeeded)
                {

                    return RedirectToAction("GetAllUsers");
                }


            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateUser(int id)
        {
            var values = _userManager.Users.FirstOrDefault(x => x.Id == id);
            AddUserVM model = new AddUserVM
            {
                Id = values.Id,
                FirstName = values.FirstName,
                LastName = values.LastName,
                UserName = values.UserName,
                Email = values.Email,
                Password = values.Password,


            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(AddUserVM model)
        {
            var values = _userManager.Users.FirstOrDefault(x => x.Id == model.Id);
            {
                values.FirstName = model.FirstName;
                values.LastName = model.LastName;
                values.UserName = model.UserName;
                values.Email = model.Email;
                values.Password = model.Password;

            };
            if (ModelState.IsValid)
            {
                IdentityResult result = await _userManager.UpdateAsync(values);
                if (result.Succeeded)
                {

                    return RedirectToAction("GetAllUsers");
                }

            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int Id)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == Id);
            await _userManager.DeleteAsync(user);
            return RedirectToAction("GetAllUsers");
        }

       


    }
}

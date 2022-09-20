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

    public class AppUsersController : Controller
    {

        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        IMemoryCache _memoryCache;
        const string cacheKey = "customerKey";
        private readonly ILogger<AppUsersController> _logger;
        public AppUsersController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IMemoryCache memoryCache, ILogger<AppUsersController> logger)

        {
            _userManager = userManager;
            _signInManager = signInManager;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = "GetAllUsers")]
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
            _logger.LogInformation("User lar listelendi.");
            return View(users);
        }

        [HttpGet]
        [Authorize(Policy = "AddUser")]
        public IActionResult AddUser()
        {

            return View();
        }

        [HttpPost]
        [Authorize(Policy = "AddUser")]
        public async Task<IActionResult> AddUser(AddUserVM model)
        {
            try
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

                    var result = await _userManager.CreateAsync(appUser, model.Password);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Yeni user eklendi.");
                        return RedirectToAction("GetAllUsers");
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "UpdateUser")]
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
        [Authorize(Policy = "UpdateUser")]
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
                    _logger.LogInformation(values.FirstName+ " "+ values.LastName + "adlı user güncellendi.");
                    return RedirectToAction("GetAllUsers");
                }

            }
            return View();
        }

        [Authorize(Policy = "DeleteUser")]
        public async Task<IActionResult> DeleteUser(int Id)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == Id);
            await _userManager.DeleteAsync(user);
            _logger.LogInformation(user.FirstName + " " + user.LastName + "adlı user silindi.");
            return RedirectToAction("GetAllUsers");
        }
    }
}

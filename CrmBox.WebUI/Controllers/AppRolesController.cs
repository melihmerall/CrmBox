using CrmBox.Core.Domain.Identity;
using CrmBox.Infrastructure.Extensions.CustomClaimType;
using CrmBox.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace CrmBox.WebUI.Controllers
{
 
    [Authorize(Roles = "Admin,Moderator")]
    public class AppRolesController : Controller
    {
        readonly RoleManager<AppRole> _roleManager;
        readonly UserManager<AppUser> _userManager;
        IMemoryCache _memoryCache;
        const string cacheKey = "customerKey";
        public AppRolesController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IMemoryCache memoryCache)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public IActionResult GetAllUserRoles()
        {
            var roles =  _roleManager.Roles.ToList();
            if (!_memoryCache.TryGetValue(cacheKey, out object list))
            {
     
                var cacheExpOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(1),
                    Priority = CacheItemPriority.Normal
                };
        
                _memoryCache.Set(cacheKey, roles, cacheExpOptions);
            }
            return View(roles);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddUserRole()
        {
            AddRoleVM model = new();
            //model.Policies = Infrastructure.Extensions.Policies.PolicyTypes.Policies.Select(x => new PolicyWithIsSelectedVM { Policy = x, IsSelected = false }).ToList();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUserRole(AddRoleVM model)
        {
            if (ModelState.IsValid)
            {
                AppRole appRole = new AppRole
                {

                    Name = model.Name,  

                };
                var result = await _roleManager.CreateAsync(appRole);
                if (result.Succeeded)
                {

                    return RedirectToAction("GetAllUserRoles");
                }


            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateUserRole(int id)
        {
            var values = _roleManager.Roles.FirstOrDefault(x => x.Id == id);
            AddRoleVM model = new AddRoleVM
            {
                Name = values.Name,

            };

            //Policyleri pasif hale getirdim.
            //model.Policies = Infrastructure.Extensions.Policies.PolicyTypes.Policies.Select(x => new PolicyWithIsSelectedVM
            //{ Policy = x, IsSelected = false }).ToList();  


            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserRole(AddRoleVM model)
        {
            var values = _roleManager.Roles.Where(x => x.Id == model.Id).FirstOrDefault();
            values.Name = model.Name;
            if (ModelState.IsValid)
            {
                var result = await _roleManager.UpdateAsync(values);
                if (result.Succeeded)
                {
                    return RedirectToAction("GetAllUserRoles");
                }
            }

            

           
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserRole(int id)
        {
            var values = _roleManager.Roles.FirstOrDefault(x => x.Id == id);
            var result = await _roleManager.DeleteAsync(values);
            if (result.Succeeded)
            {
                return RedirectToAction("GetAllUserRoles");
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignUserRole(int id)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            var roles = _roleManager.Roles.ToList();

            TempData["UserId"] = user.Id;

            var userRoles = await _userManager.GetRolesAsync(user);

            List<AssignRoleVM> model = new List<AssignRoleVM>();
            foreach (var item in roles)
            {
                AssignRoleVM m = new AssignRoleVM();
                m.RoleId = item.Id;
                m.Name = item.Name;
                m.Exist = userRoles.Contains(item.Name); // contains = eğer istenen değeri ieçriyorsa.
                model.Add(m);
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignUserRole(List<AssignRoleVM> model)
        {
            var userId = (int)TempData["UserId"];
            var user = _userManager.Users.FirstOrDefault(x => x.Id == userId);

            foreach (var item in model)
            {
                if (item.Exist)
                {
                    await _userManager.AddToRoleAsync(user, item.Name);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, item.Name);
                }
            }
            return RedirectToAction("GetAllUsers", "AppUsers");
        }

    }
}

using CrmBox.Core.Domain.Identity;
using CrmBox.Infrastructure.Extensions.CustomClaimType;
using CrmBox.Infrastructure.Extensions.Policies;
using CrmBox.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CrmBox.WebUI.Controllers
{

    [Authorize(Roles = "Root,Admin,Moderator")]
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
        [Authorize(Policy = "GetAllUserRoles")]
        public IActionResult GetAllUserRoles()
        {
            var roles = _roleManager.Roles.ToList();

            


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
        [Authorize(Policy = "AddUserRole")]
        public IActionResult AddUserRole()
        {
            AddRoleVM model = new();
            
            //model.Policies = Infrastructure.Extensions.Policies.PolicyTypes.Policies.Select(x => new PolicyWithIsSelectedVM { Policy = x, IsSelected = false }).ToList();
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "AddUserRole")]
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
        [Authorize(Policy = "UpdateUserRole")]
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
        [Authorize(Policy = "UpdateUserRole")]
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

        [Authorize(Policy = "DeleteUserRole")]
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
        [Authorize(Policy = "ChooseUserRole")]
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
                

                m.Exist = userRoles.Contains(item.Name); // contains = eğer istenen değeri içeriyorsa.
                model.Add(m);
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "ChooseUserRole")]
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
        [HttpGet]
        [Authorize(Policy = "ManageUserClaims")]
        public async Task<IActionResult> ManageUserClaims(int id)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            ViewBag.userName = user.UserName;
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View();
            }
            var existingUserClaims = await _userManager.GetClaimsAsync(user);
            var model = new SelectUserClaimsVM
            {
                UserId = id,
            };
            foreach (Claim claim in ClaimStore.AllClaims)
            {
                UserClaimVM userClaim = new UserClaimVM
                {
                    ClaimType = claim.Type
                };
                if (existingUserClaims.Any(c => c.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }
                model.Claims.Add(userClaim);
            }
            return View(model);
        }
        [HttpPost]
        [Authorize(Policy = "ManageUserClaims")]
        public async Task<IActionResult> ManageUserClaims(SelectUserClaimsVM model,int id)
        {
             var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.UserId} cannot be found";
                return View();
            }
            var claims = await _userManager.GetClaimsAsync(user);
            var result = await _userManager.RemoveClaimsAsync(user, claims);
            if (!result.Succeeded) {
                ModelState.AddModelError("", "Cannot Remove user existing Claims");
                return View(model);
            }
            result = await _userManager.AddClaimsAsync(user,
                model.Claims.Where(c => c.IsSelected).Select(c => new Claim(c.ClaimType, c.ClaimType)));
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot Remove user existing Claims");
                return View(model);
            }
            return RedirectToAction("GetAllUsers","AppUsers", new { Id = model.UserId });
        }

    }
}

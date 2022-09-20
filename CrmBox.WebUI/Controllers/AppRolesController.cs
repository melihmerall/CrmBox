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
    public class AppRolesController : Controller
    {
        readonly RoleManager<AppRole> _roleManager;
        readonly UserManager<AppUser> _userManager;
        private readonly ILogger<AppRolesController> _logger;
        IMemoryCache _memoryCache;
        const string cacheKey = "customerKey";
        public AppRolesController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IMemoryCache memoryCache, ILogger<AppRolesController> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = "GetAllUserRoles")]
        public IActionResult GetAllUserRoles()
        {
            var roles = _roleManager.Roles.ToList();
            _logger.LogInformation("Kullanıcı rol listesi çağrıldı.");
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
            try
            {
                if (ModelState.IsValid)
                {
                    AppRole appRole = new AppRole
                    {

                        Name = model.Name,


                    };

                    var result = await _roleManager.CreateAsync(appRole);
                    _logger.LogInformation("Kullanı yeni rol tanımı eklendi.");
                    if (result.Succeeded)
                    {

                        return RedirectToAction("GetAllUserRoles");
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
            try
            {
                var values = _roleManager.Roles.Where(x => x.Id == model.Id).FirstOrDefault();
                values.Name = model.Name;
                if (ModelState.IsValid)
                {
                    var result = await _roleManager.UpdateAsync(values);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Kullanıcı rol tanımı güncellendi.");
                        return RedirectToAction("GetAllUserRoles");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
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
                _logger.LogInformation("Kullanıcı rol tanımı silindi.");

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
            _logger.LogInformation("Kullanıcıya rol ekleme işlemi yapıldı.");
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
            var role = _roleManager.Roles.FirstOrDefault(x => x.Id == id);
            ViewBag.roleName = role.Name;
            if (role == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View();
            }
            var existingUserClaims = await _roleManager.GetClaimsAsync(role);
            var model = new SelectUserClaimsVM
            {
                RoleId = id,
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
        public async Task<IActionResult> ManageUserClaims(SelectUserClaimsVM model, int id)
        {
            var role = _roleManager.Roles.FirstOrDefault(x => x.Id == id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.RoleId} cannot be found";
                return View();
            }

            // bu kısım claimleri kaldırmak için.
            var claims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in claims)
            {
                var result = await _roleManager.RemoveClaimAsync(role, claim);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot Remove user existing Claims");
                    return View(model);
                }
            }

            // Bu kısım viewde seçilenleri yakalayıp foreach ile dönerek databaseye eklemek için
            var claimss = model.Claims.Where(c => c.IsSelected).Select(c => new Claim(c.ClaimType, c.ClaimType));
            
            foreach (var claim in claimss)
            {
                var result = await _roleManager.AddClaimAsync(role, claim);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot Remove user existing Claims");
                    return View(model);
                }

            }
            _logger.LogInformation("User claim ekleme işlemi yapıldı.");
            return RedirectToAction("GetAllUserRoles", "AppRoles");

        }
    }

}

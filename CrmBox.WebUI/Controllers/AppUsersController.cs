using CrmBox.Core.Domain.Identity;
using CrmBox.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CrmBox.WebUI.Controllers
{
    [Authorize(Roles = "Admin,Moderator")]
    public class AppUsersController : Controller
    {
        
        readonly UserManager<AppUser> _userManager;
        readonly RoleManager<AppRole> _roleManager;
        readonly SignInManager<AppUser> _signInManager;
        public AppUsersController(SignInManager<AppUser> signInManager,UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
             
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [HttpGet]


        public IActionResult GetAllUsers()
        {

            var users = _userManager.Users.ToList();
            //var userRole = _roleManager.Roles.Select(x => x.Name).FirstOrDefault();
            //ViewBag.userRole = userRole;
            return View(users);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddUser()
        {
            AddUserVM appUser = new();

            var Roles = _roleManager.Roles.Select(x => new RoleWithSelectVM
            {
                Id = x.Id,
                Name = x.Name,
                IsSelected = false,
                Claims = _roleManager.GetClaimsAsync(x).Result
            }).ToList();

            appUser.Roles = Roles;
            return View(appUser);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUser(AddUserVM model)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new()
                {
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,

                };

                IdentityResult result = await _userManager.CreateAsync(appUser, model.Password);

                if (result.Succeeded)
                {

                    //foreach (var role in model.Roles.Where(x => x.IsSelected = true))
                    //{
                    //    await _userManager.AddToRoleAsync(appUser, role.Name);
                    //}
                    return RedirectToAction("GetAllUsers", "AppUsers");
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateUser(int id)
        {
            var values = _userManager.Users.FirstOrDefault(x => x.Id == id);
            UpdateUserVM model = new UpdateUserVM
            {
                Id = values.Id,
                FirstName = values.FirstName,
                LastName = values.LastName,
                UserName = values.UserName,
                Password = values.PasswordHash,   
                
                
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(UpdateUserVM model)
        {

            var values = _userManager.Users.Where(x => x.Id == model.Id).FirstOrDefault();
            values.UserName = model.UserName;
            values.FirstName = model.FirstName;
            values.LastName = model.LastName;
            values.PasswordHash = model.Password;
            
            var result = await _userManager.UpdateAsync(values);

            if (result.Succeeded)
            {
                return RedirectToAction("GetAllUsers");
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int Id)
        {
            var user = _userManager.Users.FirstOrDefault(x=>x.Id==Id);
            await _userManager.DeleteAsync(user);
            return RedirectToAction("GetAllUsers");
        }
    }
}

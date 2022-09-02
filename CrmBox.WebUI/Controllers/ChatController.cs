using CrmBox.Core.Domain.Identity;
using CrmBox.Persistance.Context;
using CrmBox.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CrmBox.WebUI.Controllers
{
    [Authorize(Roles ="Admin,Moderator,Root")]
    public class ChatController : Controller
    {
        private readonly CrmBoxIdentityContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ChatController(CrmBoxIdentityContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()    
        {
            return View();
        }
        public IActionResult UserCount()
        {
            return View();
        }
        [Authorize(Policy ="Chat")]
        public IActionResult Chat()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = _userManager.Users.FirstOrDefault(x=>x.Id.ToString()==userId).UserName;
            ChatVM chatVm = new()
            {
                
                UserId = userId,
            };
            ViewBag.userName = userName;
            return View(chatVm);
        }


    }
}

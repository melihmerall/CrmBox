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
        [Authorize(Policy = "CustomerChatSupport")]
        public IActionResult SupportAgent()
        {
            return View();
        }

        public IActionResult UserCount()
        {
            return View();
        }

    }
}

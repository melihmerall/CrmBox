using CrmBox.Persistance.Context;
using CrmBox.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CrmBox.WebUI.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly CrmBoxIdentityContext _context;

        public ChatController(CrmBoxIdentityContext context)
        {
            _context = context;
        }

        public IActionResult Index()    
        {
            return View();
        }
        public IActionResult UserCount()
        {
            return View();
        }
        public IActionResult Chat()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            ChatVM chatVm = new()
            {
                
                MaxRoomAllowed = 4,
                UserId = userId,
            };
            return View(chatVm);
        }


    }
}

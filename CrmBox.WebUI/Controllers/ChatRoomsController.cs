using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrmBox.WebUI.Models;
using CrmBox.Persistance.Context;
using CrmBox.Core.Domain;

namespace CrmBox.WebUI.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class ChatRoomsController : ControllerBase
    {
        private readonly CrmBoxIdentityContext _context;

        public ChatRoomsController(CrmBoxIdentityContext context)
        {
            _context = context;
        }

      

        [HttpGet]
        [Route("/[controller]/GetChatUser")]
        public async Task<ActionResult<Object>> GetChatUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var users = await _context.Users.ToListAsync();

            if (users == null)
            {
                return NotFound();
            }

            return users.Where(u => u.Id.ToString() != userId).Select(u => new { u.Id, u.UserName }).ToList();
        }

    
        
    }
}

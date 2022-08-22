using CrmBox.Core.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CrmBox.WebUI.Controllers
{
    public class AdminsController : Controller
    {
        readonly UserManager<AppUser> _userManager;

        public AdminsController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult GetAll()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(int v)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Update(int adminId)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Update()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int adminId)
        {
            return View();
        }

    }
}

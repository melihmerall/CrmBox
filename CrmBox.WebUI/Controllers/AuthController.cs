using CrmBox.Core.Domain.Identity;
using CrmBox.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Web;
using CrmBox.WebUI.Helper;
using System.ComponentModel.DataAnnotations;

namespace CrmBox.WebUI.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        readonly SignInManager<AppUser> _signInManager;
        readonly UserManager<AppUser> _userManager;
        readonly RoleManager<AppRole> _roleManager;


        public AuthController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        [HttpGet]
        public async Task<IActionResult> Login()
        {
            var roots = await _userManager.GetUsersInRoleAsync("root");
            if (roots.Count == 0)
            {
                AppUser rootUser = new() { FirstName = "root", LastName = "root", UserName = "root" };
                IdentityResult result = await _userManager.CreateAsync(rootUser, "pswrd");
                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(rootUser, "ROOT");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserForLoginVM vM)
        {

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(vM.Username, vM.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("GetAllCustomers", "Customers");
                }
                else
                    return View();

            }
            return View();

        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Auth");
        }


        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var link = $"<a target=\"_blank\"" +
                        $" href=\"https://localhost:7001" +
                        $"{Url.Action("UpdatePassword", "Auth", new { userId = user.Id, token = HttpUtility.UrlEncode(resetToken) })}" +
                        $"\">Yeni şifre talebi için tıklayınız.</a>";
                    EmailHelper emailHelper = new EmailHelper();
                    emailHelper.SendEmailPasswordReset(model.Email, link);
                    ViewBag.State = true;
                }
                else { ViewBag.State = false; }
                
            }
        


            //AppUser user = await _userManager.FindByEmailAsync(model.Email);
            //if (user != null)
            //{
            //    string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            //    MailMessage mail = new MailMessage();
            //    mail.IsBodyHtml = true;
            //    mail.To.Add(user.Email);
            //    mail.From = new MailAddress("melih16-meral@hotmail.com", "Şifre Güncelleme", System.Text.Encoding.UTF8);
            //    mail.Subject = "Şifre Güncelleme Talebi - CrmBox";
            //    mail.Body = $"<a target=\"_blank\" href=\"https://localhost:7001{Url.Action("UpdatePassword", "Auth", new { userId = user.Id, token = HttpUtility.UrlEncode(resetToken) })}\">Yeni şifre talebi için tıklayınız.</a>";
            //    mail.IsBodyHtml = true;
            //    SmtpClient smp = new SmtpClient();
            //    smp.UseDefaultCredentials = false;
            //    smp.Credentials = new NetworkCredential("melih16-meral@hotmail.com", "Ed4b122ff.");
            //    smp.Port = 587;
            //    smp.Host = "smtp-mail.outlook.com";
            //    smp.EnableSsl = true;
            //    smp.Send(mail);

            //    
            //}
            //else
            //    ViewBag.State = false;

            return View();
        }
        [HttpGet("[action]/{userId}/{token}")]
        public IActionResult UpdatePassword(string userId, string token)
        {
            return View();
        }
        [HttpPost("[action]/{userId}/{token}")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordVM model, string userId, string token)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            IdentityResult result = await _userManager.ResetPasswordAsync(user, HttpUtility.UrlDecode(token), model.Password);
            if (result.Succeeded)
            {
                ViewBag.State = true;
                await _userManager.UpdateSecurityStampAsync(user);
            }
            else
                ViewBag.State = false;
            return View();
        }



    }
}

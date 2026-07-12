using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

namespace DrKchida.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "/Admin")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string returnUrl = "/Admin")
        {
            var adminUsername = _configuration["AdminSettings:Username"] ?? "admin";
            var adminPassword = _configuration["AdminSettings:Password"] ?? "DrKchida2026!";

            if (username == adminUsername && password == adminPassword)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity), authProperties);

                return LocalRedirect(returnUrl);
            }

            ViewBag.Error = "Nom d'utilisateur ou mot de passe incorrect.";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Index", "Home");
        }
    }
}

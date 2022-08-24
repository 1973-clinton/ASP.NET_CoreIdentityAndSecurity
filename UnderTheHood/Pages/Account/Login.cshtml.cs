using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UnderTheHood.Models;

namespace UnderTheHood.Pages.Account
{
    //[Authorize]
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Credential Credential { get; set; }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();

            if (Credential.UserName.ToLower() == "admin" && Credential.Password == "password")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Email, "admin@yahoo.com"),
                    new Claim("Department", "HR"),
                    new Claim("Admin", "true"),
                    new Claim("Manager", "true"),
                    new Claim("EmploymentDate", "2022-01-01")
                };

                var identity = new ClaimsIdentity(claims, "CookieAuth");
                var claimsPrincipal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties()
                {
                    IsPersistent = Credential.RememberMe
                };

                await HttpContext.SignInAsync("CookieAuth", claimsPrincipal, authProperties);
            }
            return RedirectToPage("/Index");
        }
        public void OnGet()
        {
        }
    }
}

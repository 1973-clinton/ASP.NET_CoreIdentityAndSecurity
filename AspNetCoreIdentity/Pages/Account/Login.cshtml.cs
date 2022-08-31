using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreIdentity.Data.User;
using AspNetCoreIdentity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreIdentity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly UserManager<ApplicationUser> _userManager;

        public LoginModel(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
            //_userManager = userManager;
        }

        [BindProperty] 
        public CredentialVM CredentialVm { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var result =
                await _signInManager.PasswordSignInAsync(CredentialVm.Email, CredentialVm.Password,
                    CredentialVm.RememberMe, false);
                if (result.Succeeded)
                {
                   return RedirectToPage("/Index");
                }
                else
                {
                    ModelState.AddModelError("Login", result.IsLockedOut ? "You are locked out" : "Login failed");
                }
            return Page();

        }
    }
}

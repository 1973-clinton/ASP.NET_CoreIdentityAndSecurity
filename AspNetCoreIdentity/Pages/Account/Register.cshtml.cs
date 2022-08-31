using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreIdentity.Data.User;
using AspNetCoreIdentity.Services;
using AspNetCoreIdentity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreIdentity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        public RegisterModel(UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        [BindProperty]
        public RegisterVm RegisterVm { get; set; } 
        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = new ApplicationUser()
            {
                Email = RegisterVm.Email,
                UserName = RegisterVm.Email,
                Position = RegisterVm.Position,
                Department = RegisterVm.Department                
            };

            var departmentClaim = new Claim("Department", RegisterVm.Department);
            var positionClaim = new Claim("Position", RegisterVm.Position);

            var result = await _userManager.CreateAsync(user, RegisterVm.Password);
            if (result.Succeeded)
            {
                await _userManager.AddClaimAsync(user, departmentClaim);
                await _userManager.AddClaimAsync(user, positionClaim);
                var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink =  Url.PageLink(pageName:"/Account/ConfirmEmail", values: new {userId = user.Id, token = confirmationToken});

                await _emailService.Send("clinton.boamah@outlook.com",
                    user.Email,
                    "Confirm Email Address",
                    $"Click this link to confirm your email address {confirmationLink}");

                return RedirectToPage("/Account/Login");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }
                return Page();
            }
            
        }
    }
}

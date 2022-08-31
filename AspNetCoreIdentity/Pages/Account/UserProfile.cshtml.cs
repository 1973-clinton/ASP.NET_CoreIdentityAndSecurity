using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreIdentity.Data.User;
using AspNetCoreIdentity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCoreIdentity.Pages.Account
{
    public class UserProfileModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public UserProfileVm UserProfile { get; set; }

        [BindProperty]
        public string SuccessMessage { get; set; }
        public UserProfileModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            UserProfile = new UserProfileVm();
            SuccessMessage = string.Empty;
        }
        public async Task<IActionResult> OnGet()
        {

            var (user, positionClaim, departmentClaim) = await GetUserInfoAsync();
            UserProfile.Email = User.Identity.Name;
            UserProfile.Department = departmentClaim?.Value;
            UserProfile.Position = positionClaim?.Value;

                return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            try
            {
                var (user, positionClaim, departmentClaim) = await GetUserInfoAsync();
                await _userManager.ReplaceClaimAsync(user, positionClaim,
                    new Claim(positionClaim.Type, UserProfile.Position));
                await _userManager.ReplaceClaimAsync(user, departmentClaim,
                    new Claim(departmentClaim.Type, UserProfile.Department));
            }
            catch 
            {
                ModelState.AddModelError("UserProfile", "An error occurred. Contact admin");
            }

            SuccessMessage = "Your info is successfully updated";
            return Page();
        }

        private async Task<(ApplicationUser, Claim, Claim)> GetUserInfoAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var claims = await _userManager.GetClaimsAsync(user);
            var departmentClaim = claims.FirstOrDefault(x => x.Type == "Department");
            var positionClaim = claims.FirstOrDefault(x => x.Type == "Position");

            return (user, positionClaim, departmentClaim);
        }
    }
}

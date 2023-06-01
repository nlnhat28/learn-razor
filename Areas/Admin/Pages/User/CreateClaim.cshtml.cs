using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RAZOR_EF.Models;

namespace RAZOR_EF.Areas.Admin.User
{
    public class CreateClaimModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        
        [TempData]
        public string StatusMessage { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [Required]
            [StringLength(255)]
            public string Type { get; set; }
            [Required]
            [StringLength(255)]
            public string Value { get; set; }
        }

        public CreateClaimModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public AppUser User { get; set; }

        public async Task<IActionResult> OnGet(string userId)
        {
            if (userId == null) return NotFound();
            User = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (User == null) return NotFound();
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(string userId)
        {
            if (userId == null) return NotFound();
            if (!ModelState.IsValid) return Page();
            User = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (User == null) return NotFound();
            if (!ModelState.IsValid) return Page();

            if ((await _userManager.GetClaimsAsync(User)).Any(c => c.Type == Input.Type && c.Value == Input.Value))
            {
                ModelState.AddModelError(string.Empty, "Claim already exists.");
                return Page();
            }
            var claim = new Claim(Input.Type, Input.Value);
            var result = await _userManager.AddClaimAsync(User, claim);
            if (result.Succeeded)
            {
                StatusMessage = $"Claim '{claim.Type}: {claim.Value}' has been added for user {User.UserName}.";
                return RedirectToPage("./EditRole", new { id = userId });
            }
            else
            {
                result.Errors.ToList().ForEach(e =>
                {
                    ModelState.AddModelError(string.Empty, e.Description);
                });
                return Page();
            }
        }
    }
}

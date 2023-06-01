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

namespace RAZOR_EF.Areas.Admin.Role
{
    public class CreateClaimModel : PageModel
    {
        private readonly RoleManager<AppRole> _roleManager;
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

        public CreateClaimModel(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public AppRole Role { get; set; }

        public async Task<IActionResult> OnGet(string roleId)
        {
            if (roleId == null) return NotFound();
            Role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
            if (Role == null) return NotFound();
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(string roleId)
        {
            if (roleId == null) return NotFound();
            if (!ModelState.IsValid) return Page();
            Role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
            if (Role == null) return NotFound();
            if (!ModelState.IsValid) return Page();

            if ((await _roleManager.GetClaimsAsync(Role)).Any(c => c.Type == Input.Type && c.Value == Input.Value))
            {
                ModelState.AddModelError(string.Empty, "Claim already exists.");
                return Page();
            }
            var claim = new Claim(Input.Type, Input.Value);
            var result = await _roleManager.AddClaimAsync(Role, claim);
            if (result.Succeeded)
            {
                StatusMessage = $"Claim '{claim.Type}: {claim.Value}' has been added to role {Role.Name}.";
                return RedirectToPage("./Edit", new { id = roleId });
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

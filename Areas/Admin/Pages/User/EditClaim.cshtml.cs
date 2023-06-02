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
using App.Models;

namespace App.Areas.Admin.User
{
    public class EditClaimModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;

        private readonly AppDbContext _context;
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

        public EditClaimModel(UserManager<AppUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public IdentityUserClaim<string> Claim { get; set; }

        public async Task<IActionResult> OnGet(string claimId)
        {
            if (claimId == null) return NotFound();
            Claim = await (_context.UserClaims.Where(c => c.Id.ToString() == claimId)).FirstOrDefaultAsync();
            if (Claim == null) return NotFound();
            Input = new InputModel
            {
                Type = Claim.ClaimType,
                Value = Claim.ClaimValue
            };
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(string claimId)
        {
            if (claimId == null) return NotFound();
            if (!ModelState.IsValid) return Page();

            Claim = await (_context.UserClaims.Where(c => c.Id.ToString() == claimId)).FirstOrDefaultAsync();
            if (Claim == null) return NotFound();
            
            if (_context.UserClaims.Any(c => c.Id != Claim.Id && c.ClaimType == Input.Type && c.ClaimValue == Input.Value))
            {
                ModelState.AddModelError(string.Empty, "Claim already exists.");
                return Page();
            }

            Claim.ClaimType = Input.Type;
            Claim.ClaimValue = Input.Value;
            _context.Attach(Claim).State = EntityState.Modified;
            try 
            {
                await _context.SaveChangesAsync();
                StatusMessage = $"Claim '{Claim.ClaimType}: {Claim.ClaimValue}' has been updated.";
                return RedirectToPage("./EditRole", new { id = Claim.UserId});
            }
            catch(Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return Page();
            }
        }
        public async Task<IActionResult> OnPostDeleteAsync(string claimId)
        {
            if (claimId == null) return NotFound();
            if (!ModelState.IsValid) return Page();

            Claim = await (_context.UserClaims.Where(c => c.Id.ToString() == claimId)).FirstOrDefaultAsync();
            if (Claim == null) return NotFound();

            try
            {    
                _context.Remove(Claim);
                await _context.SaveChangesAsync();
                StatusMessage = $"Claim '{Claim.ClaimType}: {Claim.ClaimValue}' has been deleted.";
                return RedirectToPage("./EditRole", new { id = Claim.UserId });
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return Page();
            }
        }
    }
}

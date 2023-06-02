using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using App.Models;

namespace App.Areas.Admin.Role
{
    public class DeleteModel : PageModel
    {
        private readonly RoleManager<AppRole> _roleManager;

        public DeleteModel(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [BindProperty]
        public AppRole Role { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == id);

            if (Role == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == id);

            if (Role != null)
            {
                await _roleManager.DeleteAsync(Role);
            }

            return RedirectToPage("./Index");
        }
    }
}

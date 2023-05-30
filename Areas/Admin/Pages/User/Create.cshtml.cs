using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RAZOR_EF.Models;

namespace RAZOR_EF.Areas.Admin.User
{
    public class CreateModel : PageModel
    {
        private readonly RoleManager<AppRole> _roleManager;
        public CreateModel(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public AppRole Role { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Role.CreateTime = DateTime.Now;
            await _roleManager.CreateAsync(Role);

            return RedirectToPage("./Index");
        }
    }
}

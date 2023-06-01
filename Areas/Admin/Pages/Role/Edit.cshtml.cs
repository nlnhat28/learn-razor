using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class EditModel : PageModel
    {
        private readonly BlogDbContext _context;
        private readonly RoleManager<AppRole> _roleManager;

        public EditModel(RoleManager<AppRole> roleManager, BlogDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        [DisplayName("Claims")]
        public List<IdentityRoleClaim<string>> Claims { get; set; }

        public AppRole Role { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel 
        {
            [DisplayName("Name")]
            [Required]
            [StringLength(256)]
            [DataType(DataType.Text)]
            public string Name { get; set;}

            [DisplayName("Description")]
            [DataType(DataType.Text)]
            public string? Description { get; set; }
        }


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
            Input = new InputModel()
            {
                Name = Role.Name,
                Description = Role.Description
            };
            Claims = await _context.RoleClaims.Where(c => c.RoleId == Role.Id).ToListAsync();
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == id);

            if (Role == null)
            {
                return NotFound();
            }
            Role.Name = Input.Name;
            Role.Description = Input.Description;
            await _roleManager.UpdateAsync(Role);
            return RedirectToPage("./Index");
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RAZOR_EF.Models;

namespace RAZOR_EF.Areas.Admin.User
{
    public class EditModel : PageModel
    {
        private readonly RoleManager<AppRole> _roleManager;

        public EditModel(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

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

            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == id);

            if (role == null)
            {
                return NotFound();
            }
            Input = new InputModel()
            {
                Name = role.Name,
                Description = role.Description
            };
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

            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == id);

            if (role == null)
            {
                return NotFound();
            }
            role.Name = Input.Name;
            role.Description = Input.Description;
            await _roleManager.UpdateAsync(role);
            return RedirectToPage("./Index");
        }
    }
}

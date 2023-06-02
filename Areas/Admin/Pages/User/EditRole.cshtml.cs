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
using App.Models;

namespace App.Areas.Admin.User
{
    public class AddRoleModel : PageModel
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        public AddRoleModel(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager, AppDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public AppUser User { get; set; }

        [BindProperty(SupportsGet = true)]
        [DisplayName("Roles")]
        public string[] Roles { get; set; }

        public SelectList AllRoles { get; set; }

        [DisplayName("Role claims")]
        public List<IdentityRoleClaim<string>> RoleClaims { get; set; }

        [DisplayName("User claims")]
        public List<IdentityUserClaim<string>> UserClaims { get; set; }

        [TempData]
        public string StatusMessage { get; set;}
        
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null) return NotFound();

            User = await _userManager.Users.FirstOrDefaultAsync(r => r.Id == id);

            if (User == null) return NotFound();
 
            var listRoles = await (from r in _context.Roles
                                   from ur in _context.UserRoles
                                   where r.Id == ur.RoleId
                                   && User.Id == ur.UserId
                                   select r).ToListAsync();

            if (listRoles != null && listRoles.Count != 0)
            {
                RoleClaims = new();
                foreach (var r in listRoles)
                {
                    var claims = await (from rc in _context.RoleClaims
                                        where rc.RoleId == r.Id
                                        select rc).ToListAsync();
                    RoleClaims.AddRange(claims);
                }
            }

            UserClaims = await (from c in _context.UserClaims
                                where c.UserId == User.Id
                                select c).ToListAsync();

            Roles = (await _userManager.GetRolesAsync(User)).ToArray();

            var allRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            AllRoles = new SelectList(allRoles);

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

            User = await _userManager.Users.FirstOrDefaultAsync(r => r.Id == id);

            if (User == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var oldRoles = (await _userManager.GetRolesAsync(User)).ToArray();
            var deletedRoles = oldRoles.Where(r => !Roles.Contains(r));
            var addedRoles = Roles.Where(r => !oldRoles.Contains(r));

            var allRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            AllRoles = new SelectList(allRoles);

            var resultDelete = await _userManager.RemoveFromRolesAsync(User, deletedRoles);
            if (!resultDelete.Succeeded)
            {
                resultDelete.Errors.ToList().ForEach(e =>
                {
                    ModelState.AddModelError(string.Empty, e.Description);
                });
                return Page();
            }
            var resultAdd = await _userManager.AddToRolesAsync(User, addedRoles);
            if (!resultAdd.Succeeded)
            {
                resultAdd.Errors.ToList().ForEach(e =>
                {
                    ModelState.AddModelError(string.Empty, e.Description);
                });
                return Page();
            }
            StatusMessage = $"Successfully updated role for {User.UserName}";
            return RedirectToPage("./Index");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RAZOR_EF.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace RAZOR_EF.Areas.Admin.User
{
    [Authorize(Roles = "Admin")]
    [Authorize(Roles = "Editor")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        public IndexModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IList<AppUser> Users { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
        public string StringRoles { get; set; }

        // [BindProperty(SupportsGet = true)]
        // public string[] Roles { get; set; }

        public int TotalUser { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        public readonly int itemPerPage = 10;

        [BindProperty(SupportsGet = true, Name = "p")]
        public int currentPage { get; set; } = 1;
        public int countPages { get; set; }

        public async Task OnGetAsync()
        {
            if (string.IsNullOrWhiteSpace(Search))
            {
                var totalUser = _userManager.Users.OrderByDescending(u => u.CreateTime);
                await GetArticlePerPageAsync(totalUser);
            }
            else
            {
                var TotalUser = _userManager.Users.Where(r => r.UserName.ToLower().Contains(Search.ToLower()))
                                            .OrderByDescending(r => r.CreateTime);
                await GetArticlePerPageAsync(TotalUser);
            }
        }
        public async Task GetArticlePerPageAsync(IQueryable<AppUser>totalUser)
        {
            TotalUser = await totalUser.CountAsync();
            countPages = Math.Min(TotalUser, (int)Math.Ceiling((double)TotalUser / itemPerPage));
            Users = await totalUser.Skip((currentPage - 1) * itemPerPage).Take(itemPerPage).ToListAsync();
        }
        public async Task<string> GetRolesUserAsync(AppUser user)
        {
            var listRole = await _userManager.GetRolesAsync(user);
            if (listRole == null)
                return "No roles";
            else
                // return string.Join(", ", listRole);
                return listRole.Count.ToString();
        }
    }
}

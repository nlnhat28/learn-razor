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

namespace RAZOR_EF.Areas.Admin.Role
{
    [Authorize(Roles = "Admin")]
    [Authorize(Roles = "Editor")]
    public class IndexModel : PageModel
    {
        private readonly RoleManager<AppRole> _roleManager;
        public IndexModel(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IList<AppRole> Roles { get; set; }
        public int TotalRole { get; set; }

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
                var totalRole = _roleManager.Roles.OrderByDescending(r => r.CreateTime);
                await GetArticlePerPageAsync(totalRole);
            }
            else
            {
                var totalRole = _roleManager.Roles.Where(r => r.Name.ToLower().Contains(Search.ToLower()))
                                            .OrderByDescending(r => r.CreateTime);
                await GetArticlePerPageAsync(totalRole);
            }
        }
        public async Task GetArticlePerPageAsync(IQueryable<AppRole> totalRole)
        {
            TotalRole = await totalRole.CountAsync();
            countPages = Math.Min(TotalRole, (int)Math.Ceiling((double)TotalRole / itemPerPage));
            Roles = await totalRole.Skip((currentPage - 1) * itemPerPage).Take(itemPerPage).ToListAsync();
        }
    }
}

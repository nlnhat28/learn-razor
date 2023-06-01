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
using System.Security.Claims;

namespace RAZOR_EF.Areas.Admin.User
{
    [Authorize(Roles = "Admin")]
    [Authorize(Roles = "Editor")]
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly BlogDbContext _context;
        public IndexModel(UserManager<AppUser> userManager, BlogDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public class ThisUser : AppUser
        {
            public string stringRoles { get; set; }
            public string stringRoleClaims { get; set; }
            public string stringUserClaims { get; set; }
        }
        public List<ThisUser> Users { get; set; }
        public IQueryable<AppRole> ListRoles { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

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
            var users = (from u in totalUser
                         select new ThisUser()
                         {
                            Id = u.Id,
                            UserName = u.UserName,
                            FullName = u.FullName,
                         });
            Users = await users.Skip((currentPage - 1) * itemPerPage).Take(itemPerPage).ToListAsync();
            foreach(var u in Users)
            {
                u.stringRoles = await GetRolesUserAsync(u);
                u.stringRoleClaims = await GetRoleClaimsAsync(u);
                u.stringUserClaims = await GetUserClaimsAsync(u);
            }
        }
        public async Task<string> GetRolesUserAsync(AppUser user)
        {
            var listRoles = await (from r in _context.Roles
                                   from ur in _context.UserRoles
                                   where r.Id == ur.RoleId
                                   && user.Id == ur.UserId
                                   select r).ToListAsync();
            if (listRoles == null || listRoles.Count() == 0)
                return @"<p class=""text-danger"">No role</p>";
            else
            {
                var listRolesName = listRoles.Select(r => r.Name);
                return string.Join(", ", listRolesName);
            }
        }
        public async Task<string> GetRoleClaimsAsync(AppUser user)
        {
            var listRoles = await (from r in _context.Roles
                                   from ur in _context.UserRoles
                                   where r.Id == ur.RoleId
                                   && user.Id == ur.UserId
                                   select r).ToListAsync();
            if (listRoles == null || listRoles.Count() == 0)
                return @"<p class=""text-danger"">No claim</p>";
            else
            {
                List<IdentityRoleClaim<string>> listClaims = new();
                foreach (var r in listRoles)
                {
                    // var claims = await _context.RoleClaims.Where(rc => rc.RoleId == r.Id).ToListAsync();
                    var claims = await (from rc in _context.RoleClaims
                                 where rc.RoleId == r.Id
                                 select rc).ToListAsync();
                    listClaims.AddRange(claims);
                }
                
                if (listClaims == null || listClaims.Count == 0)
                    return @"<p class=""text-danger"">No claim</p>";
                else
                {
                    var listClaimsName = listClaims.Select(c => $"{c.ClaimType}: {c.ClaimValue}").ToList();
                    return string.Join(", ", listClaimsName);
                }
            }
        }
        public async Task<string> GetUserClaimsAsync(AppUser user)
        {
            var listClaims = await (from c in _context.UserClaims
                                    where c.UserId == user.Id
                                    select c).ToListAsync();
            if (listClaims == null || listClaims.Count() == 0)
                return @"<p class=""text-danger"">No claim</p>";
            else
            {
                var listClaimsName = listClaims.Select(c => $"{c.ClaimType}: {c.ClaimValue}").ToList();
                return string.Join(", ", listClaimsName);
            }
        }
    }
}

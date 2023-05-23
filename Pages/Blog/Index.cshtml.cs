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

namespace RAZOR_EF.Pages_Blog
{
    public class IndexModel : PageModel
    {
        private readonly RAZOR_EF.Models.BlogDbContext _context;

        public IndexModel(RAZOR_EF.Models.BlogDbContext context)
        {
            _context = context;
        }

        public IList<Article> Article { get;set; }
        public int TotalArticle { get; set; }

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
                var totalArticle = _context.Article.OrderByDescending(a => a.CreatedTime);
                await GetArticlePerPageAsync(totalArticle);
            }
            else
            {
                var totalArticle = _context.Article.Where(a => a.Title.ToLower().Contains(Search.ToLower()));
                await GetArticlePerPageAsync(totalArticle);
            }
        }
        public async Task GetArticlePerPageAsync(IQueryable<Article> totalArticle)
        {
            TotalArticle = await totalArticle.CountAsync();
            countPages = Math.Min(TotalArticle, (int)Math.Ceiling((double)TotalArticle / itemPerPage));
            Article = await totalArticle.Skip((currentPage - 1) * itemPerPage).Take(itemPerPage).ToListAsync();
        }
    }
}

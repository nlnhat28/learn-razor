using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using App.Models;

namespace App.Pages
{
    [Authorize(Policy = "ViewAllArticle")]
    public class ArticleModel : PageModel
    {
        public AppDbContext AppDbContext{ get; set; }
        public ArticleModel (AppDbContext _AppDbContext)
        {
            AppDbContext = _AppDbContext;
        }
        public void OnGet()
        {

        }
    }
}

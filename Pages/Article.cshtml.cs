using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RAZOR_EF.Models;

namespace RAZOR_EF.Pages
{
    [Authorize(Roles = "Admin, Editor, Vip")]
    public class ArticleModel : PageModel
    {
        public BlogDbContext blogDbContext{ get; set; }
        public ArticleModel (BlogDbContext _blogDbContext)
        {
            blogDbContext = _blogDbContext;
        }
        public void OnGet()
        {

        }
    }
}

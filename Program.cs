using Microsoft.EntityFrameworkCore;
using RAZOR_EF.Models;

namespace RAZOR_EF;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddDbContext<BlogDbContext>(options =>
        {
            var connectionString = builder.Configuration.GetConnectionString("BlogDbConnectionString");
            options.UseSqlServer(connectionString);
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();

        app.Run();
    }
}

/*
    - DI options:
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) {} 

    - Register service:
        .AddDbContext<BlogDbContext>(options => {
            var connectionString = builder.Configuration.GetConnectionString("BlogDbConnectionString");
            options.UseSqlServer(connectionString);
            });

    - Save connection string in appsetting.json :
        appsetting.json 
            "ConnectionStrings": {
                "BlogDbConnectionString" : "Server = NLNHAT\\NLNHAT; Database=BlogDb; UID=sa; PWD=052800; Encrypt=False"
            }  
    
    - Insert data:
        + migration_file => migrationBuilder.InsertData()

        + Fake data : Nuget/Bogus

    - DI PageModel:
        public IndexModel(BlogDbContext _dbcontext){...}

    - Create CRUD Pages:
        + dotnet aspnet-codegenerator razorpage -m Article -dc ArticleContext -udl -outDir Pages/Blog --referenceScriptLibraries

    - Paging
        + Helpers/PagingModel.cs
                . currentPage
                . countPages
                . generateUrl = (int? pageNumber) => @Url.Page("/Blog/Index", new {Search = Search, p = pageNumber}) 
        + [BindProperty(SupportsGet = true, Name = "p")]
        + countPages = Math.Min(Article.Count, (int)Math.Ceiling((double)Article.Count / itemPerPage));
        
*/

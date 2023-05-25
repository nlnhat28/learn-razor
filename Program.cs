
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using RAZOR_EF.Models;
using RAZOR_EF.Services;
using Microsoft.AspNetCore.Identity;

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
        builder.Services.AddIdentity<AppUser, IdentityRole>()
                        .AddEntityFrameworkStores<BlogDbContext>()
                        .AddDefaultTokenProviders();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false; // Không bắt phải có số
            options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
            options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
            options.Password.RequireUppercase = false; // Không bắt buộc chữ in
            options.Password.RequiredLength = 6; // Số ký tự tối thiểu của password
            options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

            // Cấu hình Lockout - khóa user
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
            options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lần thì khóa
            options.Lockout.AllowedForNewUsers = true;

            // Cấu hình về User.
            options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;  // Email là duy nhất

            // Cấu hình đăng nhập.
            options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
            options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
        });

        builder.Services.AddOptions();
        var mailOptions = builder.Configuration.GetSection("MailSettings");
        builder.Services.Configure<MailSettings>(mailOptions);
        builder.Services.AddTransient<IEmailSender, SendMailService>();

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

        app.UseAuthentication();
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

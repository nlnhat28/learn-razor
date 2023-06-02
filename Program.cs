
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using App.Models;
using App.Services;
using Microsoft.AspNetCore.Identity;
using App.Security.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace App;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            var connectionString = builder.Configuration.GetConnectionString("BlogDbConnectionString");
            options.UseSqlServer(connectionString);
        });
        builder.Services.AddIdentity<AppUser, AppRole>()
                        .AddEntityFrameworkStores<AppDbContext>()
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
            options.SignIn.RequireConfirmedEmail = false;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
            options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
            options.SignIn.RequireConfirmedAccount = false;          // Xác thực email mới cho đăng nhập
        });

        builder.Services.AddOptions();
        var mailOptions = builder.Configuration.GetSection("MailSettings");
        builder.Services.Configure<MailSettings>(mailOptions);
        builder.Services.AddSingleton<IEmailSender, SendMailService>();

        builder.Services.AddAuthentication().AddGoogle(options =>
        {
            var config = builder.Configuration.GetSection("Authentication:Google");
            options.ClientId = config["ClientId"];
            options.ClientSecret = config["ClientSecret"];
            options.CallbackPath = "/LoginGoogle";
        });

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Login";
            options.LogoutPath = "/Logout";
            options.AccessDeniedPath = "/AccessDenied";
        });

        builder.Services.Configure<SecurityStampValidatorOptions>(options =>
        {
            // Trên 5 giây truy cập lại sẽ nạp lại thông tin User (Role)
            // SecurityStamp trong bảng User đổi -> nạp lại thông tinn Security
            options.ValidationInterval = TimeSpan.FromSeconds(5);
        });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AllowEditPolicy", policyBuilder =>
            {
                policyBuilder.RequireAuthenticatedUser();
                // policyBuilder.RequireRole("Admin", "Editor");
                // policyBuilder.RequireRole("Vip");

                policyBuilder.RequireClaim("Vip", new string[]{
                    "Blog",
                    "Article",
                    "Privacy"
                });
            });
            options.AddPolicy("ViewAllArticle", policyBuilder =>
            {
                policyBuilder.RequireAuthenticatedUser();
                policyBuilder.AddRequirements(new GenZRequirement());
            });
            options.AddPolicy("ShowAdminMenu", policyBuilder =>
            {
                policyBuilder.RequireAuthenticatedUser();
                policyBuilder.RequireRole("Admin");
            });
            options.AddPolicy("UpdateArticleRequirement", policyBuilder =>
            {
                policyBuilder.RequireAuthenticatedUser();
                policyBuilder.AddRequirements(new UpdateArticleRequirement());
            });
        });

        builder.Services.AddTransient<IAuthorizationHandler, AppAuthorizationHandler>();

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



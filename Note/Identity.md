# Package
* dotnet add package Microsoft.AspNetCore.Identity 
* dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 6.0.0
* dotnet add package Microsoft.VisualStudio.Web.CodeGeneration 
* dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design 
* dotnet add package Microsoft.AspNetCore.Identity.UI
* dotnet add package Microsoft.AspNetCore.Authentication
* dotnet add package Microsoft.AspNetCore.Http.Abstractions
* dotnet add package Microsoft.AspNetCore.Authentication.Cookies
* dotnet add package Microsoft.AspNetCore.Authentication.Facebook
* dotnet add package Microsoft.AspNetCore.Authentication.Google --version 6.0.5
* dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
* dotnet add package Microsoft.AspNetCore.Authentication.MicrosoftAccount
* dotnet add package Microsoft.AspNetCore.Authentication.oAuth
* dotnet add package Microsoft.AspNetCore.Authentication.OpenIDConnect
* dotnet add package Microsoft.AspNetCore.Authentication.Twitter

# Identity
* Authentication
* Authorization

# Middleware
```csharp
 app.UseAuthentication()
 app.UseAuthorization()
```

# Model
* `IdentityUser`, inheritance: `AppUser : IdentityUser`
* `IdentityDbContext`, inheritance: `BlogDbContext : IdentityDbContext<AppUser>`

# Register services
```csharp
builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<BlogDbContext>()
                .AddDefaultTokenProviders();
```
# ConfigIdentityOptions
```csharp
builder.Services.Configure<IdentityOptions>(options =>
{
    //options.
});
```
# ConfigTableNames [Optional]
```csharp
//OnModelCreating
foreach (var entityType in modelBuilder.Model.GetEntityTypes())
{
    var tableName = entityType.GetTableName();
    if (tableName.StartsWith("AspNet"))
    {
        entityType.SetTableName(tableName.Substring(6));
    }
}
```
# Login partial
* Path: Area/Identity/Pages/_LoginPartial.cshtml

# Url
* Identity/Account/Register
* Identity/Account/Login
* Identity/Account/Logout
* Identity/Account/Manage
* ...

# Get info of User 
```cs
@inject SignInManager<AppUser> signinManager
@inject UserManager<AppUser> userManager

@{
    signinManager.IsSignedIn(User);
    userManager.GetUserName(User);
}
```

# SendMailService
```csharp
public class SendMailService : IEmailSender {
    //
}
```

# Scaffold identity UI
`dotnet aspnet-codegenerator identity -dc MyContext`

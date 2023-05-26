# DI options:
```csharp
public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) {} 
```

# Register service:
```csharp
builder.Services.AddDbContext<BlogDbContext>(options => {
    var connectionString = builder.Configuration.GetConnectionString("BlogDbConnectionString");
    options.UseSqlServer(connectionString);
    });
```
# Save connection string in `appsetting.json` :
```json
"ConnectionStrings": {
    "BlogDbConnectionString" : "Server = NLNHAT\\NLNHAT; Database=BlogDb; UID=sa; PWD=052800; Encrypt=False"
}  
```
# Insert data:
* migration_file => `migrationBuilder.InsertData(){//}`
* Fake data : [Bogus](https://www.nuget.org/packages/Bogus/)

# DI PageModel:
```csharp
public IndexModel(BlogDbContext _dbcontext)
{
    //
}
```
# Create CRUD Pages:
`dotnet aspnet-codegenerator razorpage -m Article -dc ArticleContext -udl -outDir Pages/Blog --referenceScriptLibraries`

# Paging
* Helpers/PagingModel.cs
        . currentPage
        . countPages
        . generateUrl = (int? pageNumber) => @Url.Page("/Blog/Index", new {Search = Search, p = pageNumber}) 
* `[BindProperty(SupportsGet = true, Name = "p")]`
* `countPages = Math.Min(Article.Count, (int)Math.Ceiling((double)Article.Count / itemPerPage));`
        
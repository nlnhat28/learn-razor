# Authorize
* Add Attribute `[Authorize]`
```csharp
 [Authorize]
    public class IndexModel : PageModel
```
* ConfigureApplicationCookie
```csharp
builder.Services.ConfigureApplicationCookie(options =>
{
    //options.
});
```
# Returns
```csharp
return LocalRedirect(returnUrl);

return RedirectToPage("Login");

return RedirectToPage("./Login", new { ReturnUrl = returnUrl });

return Page();
```

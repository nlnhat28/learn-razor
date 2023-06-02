# Requirements
## Add requirements to policy
```csharp
options.AddPolicy("ViewArticleDetail", policyBuilder =>
{
    policyBuilder.AddRequirements(new GenZRequirement());
});
```
## Implement class `IAuthorizationRequirement`
```csharp
public class GenZRequirement : IAuthorizationRequirement
{
    // Requirement properties ...
}
```
## Implement class `IAuthorizationHandler`
```cs
public class AppAuthorizationHandler : IAuthorizationHandler
{
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        var requirements = context.PendingRequirements.ToList();
        foreach (var requirement in requirements)
        {
            if (requirement is GenZRequirement)
            {
                if (IsGenZ(context.User, (GenZRequirement)requirement))
                    context.Succeed(requirement);
            }
        }
        return Task.CompletedTask;
    }
}
```
**Note** : Add Service
```cs
builder.Services.AddTransient<IAuthorizationHandler, AppAuthorizationHandler>();
```

# IAuthorizationService
## Inject
* View cshtml
```cs
@inject IAuthorizationService authorizationService
```
* Page model
```cs
private readonly IAuthorizationService _authorService;
public EditModel(IAuthorizationService authorService)
{
    _authorService = authorService;
}
```
## Usage
```cs
@inject Microsoft.AspNetCore.Authorization.IAuthorizationService authorizationService
@if ((await authorizationService.AuthorizeAsync(User, "ShowAdminMenu")).Succeeded)
{
    //
}
```
## Resource-based
* Syntax
```cs
if ((await _authorService.AuthorizeAsync(User, Article, "UpdateArticleRequirement")).Succeeded)
{
   // 
}
```
* In IAuthorizationHandler
```cs
if (requirement is UpdateArticleRequirement)
{
    if (IsFromYear(context.User, context.Resource, (UpdateArticleRequirement)requirement))
        context.Succeed(requirement);
}
```
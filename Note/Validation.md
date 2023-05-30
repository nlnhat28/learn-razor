# Customize validtion message
## Inheritance class `IdentityErrorDescriber`
```csharp
 public class AppIdentityErrorDescriber : IdentityErrorDescriber
{
    public override IdentityError DuplicateRoleName(string role)
    {
        // return base.DuplicateRoleName(role);
        var er = base.DuplicateRoleName(role);
        return new IdentityError
        {
            Code = er.Code,
            Description = $"Role '{role}' is already taken."
        };
    }
}
```
## Add to service
```csharp
builder.Services.AddSingleton<IdentityErrorDescriber, AppIdentityErrorDescriber>();
```
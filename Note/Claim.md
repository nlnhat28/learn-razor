# Policy
## Add to ServiceCollection
```csharp
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AllowEditPolicy", policyBuilder =>
    {
        policyBuilder.RequireAuthenticatedUser();
        policyBuilder.RequireRole("Admin", "Editor"); // Admin or Editor
        policyBuilder.RequireRole("Vip");             // and Vip

        policyBuilder.RequireClaim("ClaimType1", new string[]{             // Required Claim 1
            "value1", 
  /*or*/    "value2",
  /*or*/    "value2"
        });
/*and*/ policyBuilder.RequireClaim("ClaimType2", new string[]{             // Required Claim 2
            "value1"
        });
    });
});
```
## Usage
```csharp
[Authorize(Policy = "AllowEditPolicy")]
```
# Claim
## Class Claim
* `class Claim`
    - `c.Type` : Claim name
    - `c.Value` : Claim value
* `IdentityRoleClaims`
* `IdentityUserClaims`
## Get claims
```csharp
await roleManager.GetClaimsAsync(role); // return Claims
await userManager.GetClaimsAsync(user); // return Claims
```
## Add role claim
```csharp
var claim = new Claim(Input.Type, Input.Value);
var result = await _roleManager.AddClaimAsync(role, claim);
```
## Add user claim
```csharp
var claim = new Claim(Input.Type, Input.Value);
var result = await _userManager.AddClaimAsync(user, claim);
```
# RoleManagerModel
```csharp
RoleManager<AppRole> // AppRole : IdentityRole
```
# AddIdentity
```csharp
Services.AddIdentity<AppUser, AppRole>()...
```
# Method
* Get roles
```csharp
var arrayRoles = (await _roleManager.GetRolesAsync(User)).ToArray<string>();
```
* Check role
```cs
 if(user.IsInRole("Admin")); 
 ```
# Select list
* class `SelectList(list)`
```csharp
var allRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
AllRoles = new SelectList(allRoles);
```
* Html helper `@Html.ListBoxFor(selectedItem, allItem, new {id = "id-select-roles", @class = "form-control"})`
```csharp
@Html.ListBoxFor(model => model.Roles, @Model.AllRoles, new {id = "id-select-roles", @class = "w-100"})
```
# Update roles
```csharp
var resultDelete = await _userManager.RemoveFromRolesAsync(User, deletedRoles);
var resultAdd = await _userManager.AddToRolesAsync(User, addedRoles);
```
# Authorize
```csharp
// Login is required
[Authorize] 

// At least one of roles is required
[Authorize(Roles = "Admin, Member")] 

// All roles are required
[Authorize(Roles = "Admin")] 
[Authorize(Roles = "Member")] 
```
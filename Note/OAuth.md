# Google
## Create an app on Google
* Access [Google Cloud](https://console.cloud.google.com/apis/dashboard?project=aspnet-387915)
* Then get `client_id` and `client_secret`

## Add it in `appsetting.json`
```json
"Authentication": {
    "Google": {
      "ClientId": "your-client-id",
      "ClientSecret": "your-client-secret"
    }
  }
```
## Add service and configure google options
```csharp
builder.Services.AddAuthentication().AddGoogle(options =>
{
    var config = builder.Configuration.GetSection("Authentication:Google");
    options.ClientId = config["ClientId"];
    options.ClientSecret = config["ClientSecret"];
    options.CallbackPath = "/LoginGoogle";
});
```
# Get info
## Get user
```csharp
var info = await _signInManager.GetExternalLoginInfoAsync();
```
## Claim
* Get email
```csharp
if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email)) //Email is already taken
{
    var externalEmail = info.Principal.FindFirstValue(ClaimTypes.Email);
}
```
* Get name
```csharp
info.Principal.Identity.Name
```
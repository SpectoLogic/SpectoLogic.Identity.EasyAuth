# SpectoLogic.Identity.EasyAuth

## General
While it is relativly easy to use Azure WebApp Authentication to make sure your users authenticate with AAD B2C, you still need to write some custom code to read all the claim information or even enrich those.

The library should make it more easy to integrate AAD B2C in your ASP.NET Core projects when you want to work with a ClaimsPrincipal object. 

## Using the middleware

### Add and configure the middleware

At the minimum you have to add two things in your starup code of your MVC application.
The options allow to create a "developer" claims principal if you are running locally as the authentication header is injected only in a configured Azure Web App.

```csharp
using SpectoLogic.Identity.EasyAuth;

builder.Services.UseEasyAuthB2C(op =>
{
#if DEBUG
    op.IsDevelopmentEnvironment = true;
#else
    op.IsDevelopmentEnvironment = false;
#endif
});
...
app.UseAuthentication();
app.UseAuthorization();
...
```
In appsettings.json you can define what the developer token should look like.

```json
{
  "EasyAuth": {
    "Email": "susi@welcome.net",
    "FamilyName": "Maier",
    "GivenName": "Susi",
    "Audience": "Some Guid",
    "Issuer": "https://b2clogin...",
    "Subject": "AAD-AppID",
    "B2CPolicy": "B2C_1_SignUpSignIn",
    "Version": "1.0"
  }
}
```
If you want to add role-claims, other claims, or add a custom token to deserialize  custom properties of (B2C allows to add custom claims) you can use the extension points i wrote. 

```csharp
using SpectoLogic.Identity.EasyAuth;
using Newtonsoft.Json;
using System.Security.Claims;

builder.Services.UseEasyAuthB2C(op =>
{
    op.TokenFactory = (json) => JsonConvert.DeserializeObject<CustomB2CToken>(json);
    op.ClaimsPrincipalFactory = (identity) => new CustomClaimsPrincipal(identity);
    op.InjectAdditionalClaims = (existingClaims) => new List<Claim>() { 
        new Claim("CustomClaim", "CustomClaim") };
#if DEBUG
    op.IsDevelopmentEnvironment = true;
#else
    op.IsDevelopmentEnvironment = false;
#endif
});
...
app.UseAuthentication();
app.UseAuthorization();
...
```

## Nuget Packages

- [SpectoLogic.Identity.EasyAuth](https://www.nuget.org/packages/SpectoLogic.Identity.EasyAuth/)


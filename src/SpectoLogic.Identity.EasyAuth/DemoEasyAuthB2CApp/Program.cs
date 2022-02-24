using DemoEasyAuthB2CApp.Security;
using SpectoLogic.Identity.EasyAuth;
using Newtonsoft.Json;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// TODO: ADD THIS CODE
builder.Services.UseEasyAuthB2C(op =>
    {
        op.TokenFactory = (json) => JsonConvert.DeserializeObject<CustomB2CToken>(json);
        op.ClaimsPrincipalFactory = (identity) => new CustomClaimsPrincipal(identity);
        op.InjectAdditionalClaims = (existingClaims) => new List<Claim>() { new Claim("CustomClaim", "CustomClaim") };
#if DEBUG
        op.IsDevelopmentEnvironment = true;
#else
        op.IsDevelopmentEnvironment = false;
#endif
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// TODO: Add the following line to get Authentication with AADB2C
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

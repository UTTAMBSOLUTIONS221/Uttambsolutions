using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddDataProtection();
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, x =>
{
    x.AccessDeniedPath = "/Account/AccessDenied/";
    x.LoginPath = "/Account/Signin/";
    x.Cookie.HttpOnly = true;
    x.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>()
    .AddScoped(x => x.GetRequiredService<IUrlHelperFactory>().GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext));

var app = builder.Build();

// Middleware to enforce HTTPS and limit request body size
app.Use(async (context, next) =>
{
    if (!context.Request.IsHttps)
    {
        var withHttps = $"https://{context.Request.Host}{context.Request.Path}";
        context.Response.Redirect(withHttps);
        return;
    }
    await next();
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Signin}/{id?}");

app.MapControllerRoute(
    name: "csr",
    pattern: "api/csr/{action=GenerateCsr}/uttambsolutions.com",
    defaults: new { controller = "Csr" });

app.Run();

using Microsoft.AspNetCore.Authentication.Cookies;
using GorevYonetimSistemi.Web.Services;
using GorevYonetimSistemi.Business.Services.Interfaces;
using GorevYonetimSistemi.Business.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();

//HttpClient servislerini ekleyin
builder.Services.AddScoped<UserApiService>();
builder.Services.AddScoped<DutyApiService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// Authentication & Authorization Middleware'ini ekleyin
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Cookie.Name = "AuthToken";  // Cookie adını belirleyin
    options.Cookie.HttpOnly = true;     // JavaScript erişimine kapalı
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // HTTPS üzerinden gönderilmesini sağlar
    options.ExpireTimeSpan = TimeSpan.FromHours(1);  // Cookie süresi
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-secret-key"))
    };
});
// Authorization ekleyin
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// JWT token doğrulaması için özel middleware
app.Use(async (context, next) =>
{
    var token = context.Request.Cookies["AuthToken"];

    if (!string.IsNullOrEmpty(token))
    {
        var tokenService = context.RequestServices.GetRequiredService<ITokenService>();

        var principal = tokenService.ValidateToken(token);

        if (principal != null)
        {
            context.User = principal; // Claims bilgilerini User nesnesine atıyoruz
        }
    }

    await next.Invoke();
});

app.UseRouting();

// Authentication ve Authorization middleware sırasını doğru yapın
app.UseAuthentication(); // Authentication önce
app.UseAuthorization(); // Authorization sonra

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}");

app.Run();

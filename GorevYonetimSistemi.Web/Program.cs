using GorevYonetimSistemi.Data.Entities;
using GorevYonetimSistemi.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<UserApiService>();
builder.Services.AddScoped<DutyApiService>();


// HttpClient için ekleme
builder.Services.AddHttpClient<UserApiService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5113/"); // API'nin temel adresi
});
builder.Services.AddHttpClient<DutyApiService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5113/"); // API'nin temel adresi
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}");

app.Run();

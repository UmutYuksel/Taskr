using GorevYonetimSistemi.Business.Services;
using GorevYonetimSistemi.Business.Services.Interfaces;
using GorevYonetimSistemi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using GorevYonetimSistemi.Business.Mappings;
using Microsoft.AspNetCore.Mvc;
using GorevYonetimSistemi.Data.Repositories.Interfaces;
using GorevYonetimSistemi.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GorevYonetimSistemi API", Version = "v1" });
});

// Get ConnectionString and Configure DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlite(connectionString);
    options.EnableSensitiveDataLogging();
    options.LogTo(Console.WriteLine, LogLevel.Information);
});

// Configure Model Validation Responses
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        var result = new
        {
            Message = "Validation failed",
            Errors = errors
        };

        return new BadRequestObjectResult(result);
    };
});

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });

// Register Business Services and Data Repositories
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDutyService, DutyService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDutyRepository, DutyRepositoy>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.ConfigureAutoMappers();

var app = builder.Build();

// Check Database Connection and Apply Migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    try
    {
        dbContext.Database.OpenConnection();
        Console.WriteLine("Database connection successful.");
        dbContext.Database.Migrate();
        Console.WriteLine("Migrations applied.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database connection failed: {ex.Message}");
    }
    finally
    {
        dbContext.Database.CloseConnection();
    }
}

// Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GorevYonetimSistemi API v1"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

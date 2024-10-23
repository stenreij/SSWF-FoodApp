using Application.Services;
using Core.DomainServices.Interfaces;
using Core.DomainServices.Services;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Configure culture to use dot as decimal separator
var cultureInfo = new CultureInfo("nl-NL");
cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IMealPackageService, MealPackageService>();
builder.Services.AddScoped<IMealPackageRepo, MealPackageEFRepo>();
builder.Services.AddScoped<IProductRepo, ProductEFRepo>();
builder.Services.AddScoped<ICanteenRepo, CanteenEFRepo>();
builder.Services.AddScoped<IStudentRepo, StudentEFRepo>();
builder.Services.AddScoped<IEmployeeRepo, EmployeeEFRepo>();

// Configure database connection strings
var defaultConnection = string.Empty;
var identityConnection = string.Empty;

if (builder.Environment.IsDevelopment())
{
    // Use local connection strings from appsettings.json during development
    defaultConnection = builder.Configuration.GetConnectionString("LocalDefaultConnection");
    identityConnection = builder.Configuration.GetConnectionString("LocalIdentityConnection");
}
else
{
    // Use environment variables for production connections
    defaultConnection = Environment.GetEnvironmentVariable("DefaultConnection");
    identityConnection = Environment.GetEnvironmentVariable("IdentityConnection");
}

// Register DbContext with appropriate connection string
builder.Services.AddDbContext<FoodAppDbContext>(options =>
    options.UseSqlServer(defaultConnection));

builder.Services.AddDbContext<FoodAppIdentityDbContext>(options =>
    options.UseSqlServer(identityConnection));

// Add Identity services
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<FoodAppIdentityDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

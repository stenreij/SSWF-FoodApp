using Application.Services;
using Core.DomainServices;
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

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IMealPackageService, MealPackageService>();
builder.Services.AddScoped<IMealPackageRepo, MealPackageEFRepo>();
builder.Services.AddScoped<IProductRepo, ProductEFRepo>();
builder.Services.AddScoped<ICanteenRepo, CanteenEFRepo>();
builder.Services.AddScoped<IStudentRepo, StudentEFRepo>();
builder.Services.AddScoped<IEmployeeRepo, EmployeeEFRepo>();

builder.Services.AddDbContext<FoodAppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<FoodAppIdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<FoodAppIdentityDbContext>();
        


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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

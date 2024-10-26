using API.GraphQL;
using Application.Services;
using Core.DomainServices.Interfaces;
using Core.DomainServices.Services;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddGraphQL();
builder.Services.AddScoped<IMealPackageRepo, MealPackageEFRepo>();
builder.Services.AddScoped<ICanteenRepo, CanteenEFRepo>();
builder.Services.AddScoped<IStudentRepo, StudentEFRepo>();
builder.Services.AddScoped<IEmployeeRepo, EmployeeEFRepo>();
builder.Services.AddScoped<IProductRepo, ProductEFRepo>();
builder.Services.AddScoped<IMealPackageService, MealPackageService>();

// Configure database connection strings
var defaultConnection = string.Empty;
var identityConnection = string.Empty;

if (builder.Environment.IsDevelopment())
{
    // Use local connection string during development
    defaultConnection = builder.Configuration.GetConnectionString("LocalDefaultConnection");
    identityConnection = builder.Configuration.GetConnectionString("IdentityConnection");

}
else
{
    // Use environment variable for production connection
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

// Configure GraphQL services
builder.Services.AddGraphQLServer()
    .AddQueryType<QueryType>()
    .AddType<MealPackageType>()
    .AddType<CanteenType>()
    .AddType<StudentType>()
    .AddType<ProductType>();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapGraphQL("/graphql");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.Run();

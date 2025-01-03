using API.GraphQL;
using Application.Services;
using Core.DomainServices.Interfaces;
using Core.DomainServices.Services;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;

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
builder.Services.AddScoped<AuthFilter>();

// Configure database connection strings
var defaultConnection = string.Empty;
var identityConnection = string.Empty;

if (builder.Environment.IsDevelopment())
{
    // Use local connection string during development
    defaultConnection = builder.Configuration.GetConnectionString("LocalDefaultConnection");
    identityConnection = builder.Configuration.GetConnectionString("LocalIdentityConnection");
}
else
{
    // Use environment variable for production connection
    defaultConnection = Environment.GetEnvironmentVariable("DefaultConnection");
    identityConnection = Environment.GetEnvironmentVariable("IdentityConnection");
}

builder.Services.AddDbContext<FoodAppDbContext>(options =>
    options.UseSqlServer(defaultConnection));

builder.Services.AddDbContext<FoodAppIdentityDbContext>(options =>
    options.UseSqlServer(identityConnection));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<FoodAppIdentityDbContext>()
    .AddDefaultTokenProviders();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Configure GraphQL services
builder.Services.AddGraphQLServer()
    .AddQueryType<QueryType>()
    .AddType<MealPackageType>()
    .AddType<CanteenType>()
    .AddType<StudentType>()
    .AddType<ProductType>();

var app = builder.Build();

app.UseRouting();

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

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapGraphQL("/graphql");
});

app.UseHttpsRedirection();

app.Run();

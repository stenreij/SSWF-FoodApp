using API.GraphQL;
using Application.Services;
using Core.DomainServices.Interfaces;
using Core.DomainServices.Services;
using Infrastructure;
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

if (builder.Environment.IsDevelopment())
{
    // Use local connection string during development
    defaultConnection = builder.Configuration.GetConnectionString("LocalDefaultConnection");
}
else
{
    // Use environment variable for production connection
    defaultConnection = Environment.GetEnvironmentVariable("DefaultConnection");
}

// Register DbContext with appropriate connection string
builder.Services.AddDbContext<FoodAppDbContext>(options =>
    options.UseSqlServer(defaultConnection));

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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL("/graphql");
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

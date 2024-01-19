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

builder.Services.AddDbContext<FoodAppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
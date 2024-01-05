using API.GraphQL;
using Core.DomainServices.Interfaces;
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

builder.Services.AddDbContext<FoodAppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddGraphQLServer()
    .AddQueryType<QueryType>()
    .AddType<MealPackageType>()
    .AddType<CanteenType>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGraphQL();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
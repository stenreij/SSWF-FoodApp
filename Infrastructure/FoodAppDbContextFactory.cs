using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure
{
    public class FoodAppDbContextFactory : IDesignTimeDbContextFactory<FoodAppDbContext>
    {
        public FoodAppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FoodAppDbContext>();

            var connectionString = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRINGFOODAPPDB");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string is not set.");
            }

            optionsBuilder.UseSqlServer(connectionString);

            return new FoodAppDbContext(optionsBuilder.Options);
        }
    }
}

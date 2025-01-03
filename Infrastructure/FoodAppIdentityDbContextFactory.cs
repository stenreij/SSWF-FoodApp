using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure
{
    public class FoodAppIdentityDbContextFactory : IDesignTimeDbContextFactory<FoodAppIdentityDbContext>
    {
        public FoodAppIdentityDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FoodAppIdentityDbContext>();

            var connectionString = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRINGIDENTITYDB");

            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = "Server=.;Database=AvansFoodAppIdentity;Trusted_Connection=True;";
            }

            optionsBuilder.UseSqlServer(connectionString);
            return new FoodAppIdentityDbContext(optionsBuilder.Options);
        }
    }
}

using Infrastructure;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

public class FoodAppDbContextFactory : IDesignTimeDbContextFactory<FoodAppDbContext>
{
    public FoodAppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FoodAppDbContext>();

        // Probeer de connection string uit omgevingsvariabelen te halen
        var connectionString = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRINGFOODAPPDB");

        // Gebruik een fallback als de omgevingsvariabele niet is ingesteld
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = "Server=.;Database=AvansFoodApp;Trusted_Connection=True;";
        }

        optionsBuilder.UseSqlServer(connectionString);
        return new FoodAppDbContext(optionsBuilder.Options);
    }
}

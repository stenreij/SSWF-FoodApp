using Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class FoodAppDbContext : DbContext
    {
        public FoodAppDbContext(DbContextOptions<FoodAppDbContext> options) : base(options)
        {
        }

        public DbSet<MealPackage> MealPackages { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Canteen> Canteens { get; set; }
        public DbSet<Student> Student { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MealPackage>()
                .Property(m => m.Price)
                .HasColumnType("decimal(18, 2)");
        }
    }
}

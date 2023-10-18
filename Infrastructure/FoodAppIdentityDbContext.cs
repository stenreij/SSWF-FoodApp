using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class FoodAppIdentityDbContext : IdentityDbContext
    {
        public FoodAppIdentityDbContext(DbContextOptions<FoodAppIdentityDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "220empl",
                Name = "employee",
                NormalizedName = "EMPLOYEE".ToUpper()
            });

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = "440stud",
                Name = "student",
                NormalizedName = "STUDENT".ToUpper()
            });
            var hasher = new PasswordHasher<IdentityUser>();

            modelBuilder.Entity<IdentityUser>().HasData(
                new IdentityUser
                {
                    Id = "1", 
                    UserName = "sten@mail.com",
                    NormalizedUserName = "STEN@MAIL.COM",
                    PasswordHash = hasher.HashPassword(null, "aA1234!"),
                },



                new IdentityUser
                {
                    Id = "20", 
                    UserName = "peter@mail.com",
                    NormalizedUserName = "PETER@MAIL.COM",
                    PasswordHash = hasher.HashPassword(null, "aA1234!"),
                });

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "220empl",
                    UserId = "20"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "440stud",
                    UserId = "1"
                });
        }
    }
}
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
            modelBuilder.Entity<IdentityUser>().HasData(
                new IdentityUser
                {
                    Id = "1", 
                    UserName = "sten@mail.com",
                    NormalizedUserName = "STEN@MAIL.COM",
                    ConcurrencyStamp = "708185df-715b-47a3-a294-f63a5d056b83",
                    SecurityStamp = "2aa1763a-e509-408c-b3ac-e5bb0305a9f1",
                    PasswordHash = "AQAAAAEAACcQAAAAEJfJxiV6mkNr+2+lQNeYOvnED0bsdiK1Q400kKs+afqaBKlFoZOeomQMOW6qUeZxMA==",
                },

                new IdentityUser
                {
                    Id = "20", 
                    UserName = "peter@mail.com",
                    NormalizedUserName = "PETER@MAIL.COM",
                    ConcurrencyStamp = "a7f1685a-1ed5-48dc-9cd3-f20187215a56",
                    SecurityStamp = "b5006bb2-04cb-4263-95e6-721b6e59a4b2",
                    PasswordHash = "AQAAAAEAACcQAAAAEAX+7XliRJg8dU2ZII0S0ttHzdVsumW3kbkaR7faORyV0X+XgnP6EUIGBtCpgRmcQQ==",
                },
                new IdentityUser
                {
                    Id = "2",
                    UserName = "student2@mail.com",
                    NormalizedUserName = "STUDENT2@MAIL.COM",
                    Email = "student2@mail.com",
                    NormalizedEmail = "STUDENT2@MAIL.COM",
                    ConcurrencyStamp = "f60cde3b-00ca-4bc6-a91f-bd1fa3e92781",
                    SecurityStamp = "814d8ecd-8241-41f5-952c-f6e607c5a269",
                    PasswordHash = "AQAAAAEAACcQAAAAEJfJxiV6mkNr+2+lQNeYOvnED0bsdiK1Q400kKs+afqaBKlFoZOeomQMOW6qUeZxMA==",
                },
                new IdentityUser
                {
                    Id = "21",
                    UserName = "employee2@mail.com",
                    NormalizedUserName = "EMPLOYEE2@MAIL.COM",
                    Email = "employee2@mail.com",
                    NormalizedEmail = "EMPLOYEE2@MAIL.COM",
                    ConcurrencyStamp = "c3b95f77-c81c-4b9a-a287-f11814a9be73",
                    SecurityStamp = "d36f45fa-1fd0-4e62-91d5-84618a764da6",
                    PasswordHash = "AQAAAAEAACcQAAAAEJfJxiV6mkNr+2+lQNeYOvnED0bsdiK1Q400kKs+afqaBKlFoZOeomQMOW6qUeZxMA==",
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
                },
                new IdentityUserRole<string>
                {
                    RoleId = "440stud",
                    UserId = "2"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "220empl",
                    UserId = "21"
                });
        }
    }
}

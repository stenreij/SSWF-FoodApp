using Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.FoodAppIdentityDb
{
    [DbContext(typeof(FoodAppIdentityDbContext))]
    [Migration("20261124152500_AddTestStudentAndEmployeeIdentityAccounts")]
    public partial class AddTestStudentAndEmployeeIdentityAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
INSERT INTO [AspNetUsers]
    ([Id], [AccessFailedCount], [ConcurrencyStamp], [Email], [EmailConfirmed],
     [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName],
     [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [SecurityStamp],
     [TwoFactorEnabled], [UserName])
VALUES
    ('2', 0, 'f60cde3b-00ca-4bc6-a91f-bd1fa3e92781', 'student2@mail.com', 0,
     0, NULL, 'STUDENT2@MAIL.COM', 'STUDENT2@MAIL.COM',
     'AQAAAAEAACcQAAAAEJfJxiV6mkNr+2+lQNeYOvnED0bsdiK1Q400kKs+afqaBKlFoZOeomQMOW6qUeZxMA==',
     NULL, 0, '814d8ecd-8241-41f5-952c-f6e607c5a269', 0, 'student2@mail.com'),
    ('21', 0, 'c3b95f77-c81c-4b9a-a287-f11814a9be73', 'employee2@mail.com', 0,
     0, NULL, 'EMPLOYEE2@MAIL.COM', 'EMPLOYEE2@MAIL.COM',
     'AQAAAAEAACcQAAAAEJfJxiV6mkNr+2+lQNeYOvnED0bsdiK1Q400kKs+afqaBKlFoZOeomQMOW6qUeZxMA==',
     NULL, 0, 'd36f45fa-1fd0-4e62-91d5-84618a764da6', 0, 'employee2@mail.com');

INSERT INTO [AspNetUserRoles] ([RoleId], [UserId])
VALUES ('440stud', '2'), ('220empl', '21');
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DELETE FROM [AspNetUserRoles] WHERE [UserId] IN ('2', '21');
DELETE FROM [AspNetUsers] WHERE [Id] IN ('2', '21');
");
        }
    }
}

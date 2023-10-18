using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.FoodAppIdentityDb
{
    public partial class ASPnetUserRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "220empl",
                column: "ConcurrencyStamp",
                value: "90846076-2167-42db-9a67-0249e29a6043");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "440stud",
                column: "ConcurrencyStamp",
                value: "96f2a474-a875-4ad4-b00e-d05c7f2b05f4");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "1", 0, "708185df-715b-47a3-a294-f63a5d056b83", null, false, false, null, null, "STEN@MAIL.COM", "AQAAAAEAACcQAAAAEJfJxiV6mkNr+2+lQNeYOvnED0bsdiK1Q400kKs+afqaBKlFoZOeomQMOW6qUeZxMA==", null, false, "2aa1763a-e509-408c-b3ac-e5bb0305a9f1", false, "sten@mail.com" },
                    { "20", 0, "a7f1685a-1ed5-48dc-9cd3-f20187215a56", null, false, false, null, null, "PETER@MAIL.COM", "AQAAAAEAACcQAAAAEAX+7XliRJg8dU2ZII0S0ttHzdVsumW3kbkaR7faORyV0X+XgnP6EUIGBtCpgRmcQQ==", null, false, "b5006bb2-04cb-4263-95e6-721b6e59a4b2", false, "peter@mail.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "440stud", "1" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "220empl", "20" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "440stud", "1" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "220empl", "20" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "20");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "220empl",
                column: "ConcurrencyStamp",
                value: "42880375-d360-456c-b418-222588e8f37e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "440stud",
                column: "ConcurrencyStamp",
                value: "b98c907a-1989-4ffb-9437-b4e5dfad24ff");
        }
    }
}

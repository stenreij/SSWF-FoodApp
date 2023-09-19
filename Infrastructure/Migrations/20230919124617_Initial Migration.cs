using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Canteen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<int>(type: "int", nullable: false),
                    WarmMeal = table.Column<bool>(type: "bit", nullable: true),
                    CanteenName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Canteen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContainsAlcohol = table.Column<bool>(type: "bit", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentNr = table.Column<int>(type: "int", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNr = table.Column<int>(type: "int", nullable: false),
                    StudyCity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MealPackages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<int>(type: "int", nullable: false),
                    CanteenId = table.Column<int>(type: "int", nullable: false),
                    PickUpDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdultsOnly = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Meal = table.Column<int>(type: "int", nullable: false),
                    ReservedByStudentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealPackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealPackages_Canteen_CanteenId",
                        column: x => x.CanteenId,
                        principalTable: "Canteen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealPackages_Student_ReservedByStudentId",
                        column: x => x.ReservedByStudentId,
                        principalTable: "Student",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MealPackageProduct",
                columns: table => new
                {
                    MealsId = table.Column<int>(type: "int", nullable: false),
                    ProductsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealPackageProduct", x => new { x.MealsId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_MealPackageProduct_MealPackages_MealsId",
                        column: x => x.MealsId,
                        principalTable: "MealPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealPackageProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MealPackageProduct_ProductsId",
                table: "MealPackageProduct",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_MealPackages_CanteenId",
                table: "MealPackages",
                column: "CanteenId");

            migrationBuilder.CreateIndex(
                name: "IX_MealPackages_ReservedByStudentId",
                table: "MealPackages",
                column: "ReservedByStudentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MealPackageProduct");

            migrationBuilder.DropTable(
                name: "MealPackages");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Canteen");

            migrationBuilder.DropTable(
                name: "Student");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class ChangedDatabaseMealType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealPackages_Canteen_CanteenId",
                table: "MealPackages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Canteen",
                table: "Canteen");

            migrationBuilder.RenameTable(
                name: "Canteen",
                newName: "Canteens");

            migrationBuilder.RenameColumn(
                name: "Meal",
                table: "MealPackages",
                newName: "MealType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Canteens",
                table: "Canteens",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MealPackages_Canteens_CanteenId",
                table: "MealPackages",
                column: "CanteenId",
                principalTable: "Canteens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealPackages_Canteens_CanteenId",
                table: "MealPackages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Canteens",
                table: "Canteens");

            migrationBuilder.RenameTable(
                name: "Canteens",
                newName: "Canteen");

            migrationBuilder.RenameColumn(
                name: "MealType",
                table: "MealPackages",
                newName: "Meal");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Canteen",
                table: "Canteen",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MealPackages_Canteen_CanteenId",
                table: "MealPackages",
                column: "CanteenId",
                principalTable: "Canteen",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

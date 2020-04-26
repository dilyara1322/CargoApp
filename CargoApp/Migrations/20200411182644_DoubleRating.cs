using Microsoft.EntityFrameworkCore.Migrations;

namespace CargoApp.Migrations
{
    public partial class DoubleRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mark",
                table: "Ratings");

            migrationBuilder.AddColumn<int>(
                name: "MarkFromCompanyToUser",
                table: "Ratings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MarkFromUserToConpany",
                table: "Ratings",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarkFromCompanyToUser",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "MarkFromUserToConpany",
                table: "Ratings");

            migrationBuilder.AddColumn<int>(
                name: "Mark",
                table: "Ratings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

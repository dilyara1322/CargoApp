using Microsoft.EntityFrameworkCore.Migrations;

namespace CargoApp.Migrations
{
    public partial class RatingsAreZero : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Rating",
                table: "Companies",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<float>(
                name: "Rating",
                table: "Clients",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Clients");

            migrationBuilder.AlterColumn<float>(
                name: "Rating",
                table: "Companies",
                type: "real",
                nullable: false,
                oldClrType: typeof(float),
                oldDefaultValue: 0f);
        }
    }
}

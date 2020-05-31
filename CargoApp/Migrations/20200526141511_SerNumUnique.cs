using Microsoft.EntityFrameworkCore.Migrations;

namespace CargoApp.Migrations
{
    public partial class SerNumUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Passports_Series_Number",
                table: "Passports",
                columns: new[] { "Series", "Number" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Passports_Series_Number",
                table: "Passports");
        }
    }
}

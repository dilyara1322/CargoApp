using Microsoft.EntityFrameworkCore.Migrations;

namespace CargoApp.Migrations
{
    public partial class BirthPlaceIsString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthPlace_Addition",
                table: "Passports");

            migrationBuilder.DropColumn(
                name: "BirthPlace_City",
                table: "Passports");

            migrationBuilder.DropColumn(
                name: "BirthPlace_Country",
                table: "Passports");

            migrationBuilder.DropColumn(
                name: "BirthPlace_Flat",
                table: "Passports");

            migrationBuilder.DropColumn(
                name: "BirthPlace_House",
                table: "Passports");

            migrationBuilder.DropColumn(
                name: "BirthPlace_Index",
                table: "Passports");

            migrationBuilder.DropColumn(
                name: "BirthPlace_Region",
                table: "Passports");

            migrationBuilder.DropColumn(
                name: "BirthPlace_Street",
                table: "Passports");

            migrationBuilder.AddColumn<string>(
                name: "BirthPlace",
                table: "Passports",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Companies",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthPlace",
                table: "Passports");

            migrationBuilder.AddColumn<string>(
                name: "BirthPlace_Addition",
                table: "Passports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BirthPlace_City",
                table: "Passports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BirthPlace_Country",
                table: "Passports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BirthPlace_Flat",
                table: "Passports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BirthPlace_House",
                table: "Passports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BirthPlace_Index",
                table: "Passports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BirthPlace_Region",
                table: "Passports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BirthPlace_Street",
                table: "Passports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

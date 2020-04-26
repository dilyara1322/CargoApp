using Microsoft.EntityFrameworkCore.Migrations;

namespace CargoApp.Migrations
{
    public partial class DeleteCoordsFromAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceivingAddress_Latitude",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ReceivingAddress_Longitude",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "SendingAddress_Latitude",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "SendingAddress_Longitude",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "BirthPlace_Latitude",
                table: "Passports");

            migrationBuilder.DropColumn(
                name: "BirthPlace_Longitude",
                table: "Passports");

            migrationBuilder.DropColumn(
                name: "Address_Latitude",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Address_Longitude",
                table: "Companies");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReceivingAddress_Latitude",
                table: "Requests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReceivingAddress_Longitude",
                table: "Requests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SendingAddress_Latitude",
                table: "Requests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SendingAddress_Longitude",
                table: "Requests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BirthPlace_Latitude",
                table: "Passports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BirthPlace_Longitude",
                table: "Passports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Address_Latitude",
                table: "Companies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Address_Longitude",
                table: "Companies",
                type: "int",
                nullable: true);
        }
    }
}

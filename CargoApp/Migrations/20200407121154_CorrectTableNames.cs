using Microsoft.EntityFrameworkCore.Migrations;

namespace CargoApp.Migrations
{
    public partial class CorrectTableNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Car_Drivers_DriverId",
                table: "Car");

            migrationBuilder.DropForeignKey(
                name: "FK_Good_Requests_RequestId",
                table: "Good");

            migrationBuilder.DropForeignKey(
                name: "FK_Passport_Clients_ClientId",
                table: "Passport");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Passport",
                table: "Passport");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Good",
                table: "Good");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Car",
                table: "Car");

            migrationBuilder.RenameTable(
                name: "Passport",
                newName: "Passports");

            migrationBuilder.RenameTable(
                name: "Good",
                newName: "Goods");

            migrationBuilder.RenameTable(
                name: "Car",
                newName: "Cars");

            migrationBuilder.RenameIndex(
                name: "IX_Good_RequestId",
                table: "Goods",
                newName: "IX_Goods_RequestId");

            migrationBuilder.RenameIndex(
                name: "IX_Car_DriverId",
                table: "Cars",
                newName: "IX_Cars_DriverId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Passports",
                table: "Passports",
                column: "ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Goods",
                table: "Goods",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cars",
                table: "Cars",
                column: "Number");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Drivers_DriverId",
                table: "Cars",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Goods_Requests_RequestId",
                table: "Goods",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Passports_Clients_ClientId",
                table: "Passports",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Drivers_DriverId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Goods_Requests_RequestId",
                table: "Goods");

            migrationBuilder.DropForeignKey(
                name: "FK_Passports_Clients_ClientId",
                table: "Passports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Passports",
                table: "Passports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Goods",
                table: "Goods");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cars",
                table: "Cars");

            migrationBuilder.RenameTable(
                name: "Passports",
                newName: "Passport");

            migrationBuilder.RenameTable(
                name: "Goods",
                newName: "Good");

            migrationBuilder.RenameTable(
                name: "Cars",
                newName: "Car");

            migrationBuilder.RenameIndex(
                name: "IX_Goods_RequestId",
                table: "Good",
                newName: "IX_Good_RequestId");

            migrationBuilder.RenameIndex(
                name: "IX_Cars_DriverId",
                table: "Car",
                newName: "IX_Car_DriverId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Passport",
                table: "Passport",
                column: "ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Good",
                table: "Good",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Car",
                table: "Car",
                column: "Number");

            migrationBuilder.AddForeignKey(
                name: "FK_Car_Drivers_DriverId",
                table: "Car",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Good_Requests_RequestId",
                table: "Good",
                column: "RequestId",
                principalTable: "Requests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Passport_Clients_ClientId",
                table: "Passport",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

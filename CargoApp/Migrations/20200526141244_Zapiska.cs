using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CargoApp.Migrations
{
    public partial class Zapiska : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Passports_Series_Number",
                table: "Passports");

            migrationBuilder.DropIndex(
                name: "IX_Cars_DriverId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "UserRegData");

            migrationBuilder.DropColumn(
                name: "IsFragile",
                table: "Goods");

            migrationBuilder.DropColumn(
                name: "MaxVolume",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "OgrnDate",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Volume",
                table: "Cars");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UserRegData",
                maxLength: 30,
                nullable: true,
                defaultValue: "Пользователь",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "Пользователь");

            migrationBuilder.AlterColumn<string>(
                name: "SendingAddress_Index",
                table: "Requests",
                type: "char(6)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReceivingAddress_Index",
                table: "Requests",
                type: "char(6)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "CurrentLongitude",
                table: "Requests",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "CurrentLatitude",
                table: "Requests",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Series",
                table: "Passports",
                type: "char(4)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Passports",
                type: "char(6)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "IssuedDate",
                table: "Passports",
                type: "Date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Passports",
                type: "char(7)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(7)",
                oldMaxLength: 7,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "Passports",
                type: "Date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Goods",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "DeliveryArea_Radius",
                table: "Drivers",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "DeliveryArea_Longitude",
                table: "Drivers",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "DeliveryArea_Latitude",
                table: "Drivers",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Area_Radius",
                table: "Companies",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Area_Longitude",
                table: "Companies",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Area_Latitude",
                table: "Companies",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address_Index",
                table: "Companies",
                type: "char(6)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Carrying",
                table: "Cars",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<float>(
                name: "Height",
                table: "Cars",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Length",
                table: "Cars",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Width",
                table: "Cars",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cars_DriverId",
                table: "Cars",
                column: "DriverId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cars_DriverId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Cars");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UserRegData",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "Пользователь",
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true,
                oldDefaultValue: "Пользователь");

            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "UserRegData",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "SendingAddress_Index",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReceivingAddress_Index",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CurrentLongitude",
                table: "Requests",
                type: "int",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CurrentLatitude",
                table: "Requests",
                type: "int",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Series",
                table: "Passports",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(4)");

            migrationBuilder.AlterColumn<string>(
                name: "Number",
                table: "Passports",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "IssuedDate",
                table: "Passports",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "Date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Passports",
                type: "nvarchar(7)",
                maxLength: 7,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(7)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "Passports",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "Date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Goods",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFragile",
                table: "Goods",
                type: "bit",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeliveryArea_Radius",
                table: "Drivers",
                type: "int",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeliveryArea_Longitude",
                table: "Drivers",
                type: "int",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeliveryArea_Latitude",
                table: "Drivers",
                type: "int",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Area_Radius",
                table: "Companies",
                type: "int",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Area_Longitude",
                table: "Companies",
                type: "int",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Area_Latitude",
                table: "Companies",
                type: "int",
                nullable: true,
                oldClrType: typeof(float),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address_Index",
                table: "Companies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "char(6)",
                oldNullable: true);

            migrationBuilder.AddColumn<float>(
                name: "MaxVolume",
                table: "Companies",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OgrnDate",
                table: "Companies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Carrying",
                table: "Cars",
                type: "real",
                nullable: false,
                oldClrType: typeof(float),
                oldNullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Volume",
                table: "Cars",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Passports_Series_Number",
                table: "Passports",
                columns: new[] { "Series", "Number" });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_DriverId",
                table: "Cars",
                column: "DriverId",
                unique: true);
        }
    }
}

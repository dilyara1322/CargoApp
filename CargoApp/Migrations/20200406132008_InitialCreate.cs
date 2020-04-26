using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CargoApp.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Rating = table.Column<float>(nullable: false),
                    MaxCarrying = table.Column<float>(nullable: false),
                    MaxVolume = table.Column<float>(nullable: false),
                    Area_Latitude = table.Column<int>(nullable: true),
                    Area_Longitude = table.Column<int>(nullable: true),
                    Area_Radius = table.Column<int>(nullable: true),
                    Address_Latitude = table.Column<int>(nullable: true),
                    Address_Longitude = table.Column<int>(nullable: true),
                    Address_Index = table.Column<string>(nullable: true),
                    Address_Country = table.Column<string>(nullable: true),
                    Address_Region = table.Column<string>(nullable: true),
                    Address_City = table.Column<string>(nullable: true),
                    Address_Street = table.Column<string>(nullable: true),
                    Address_House = table.Column<string>(nullable: true),
                    Address_Flat = table.Column<string>(nullable: true),
                    Address_Addition = table.Column<string>(nullable: true),
                    RegistrationDate = table.Column<DateTime>(nullable: false),
                    Inn = table.Column<string>(maxLength: 12, nullable: false),
                    Kpp = table.Column<string>(type: "char(9)", nullable: false),
                    Ogrn = table.Column<string>(type: "char(13)", nullable: false),
                    OgrnDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.UniqueConstraint("AK_Companies_Ogrn", x => x.Ogrn);
                    table.UniqueConstraint("AK_Companies_Inn_Kpp", x => new { x.Inn, x.Kpp });
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Salt = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true, defaultValue: "Пользователь"),
                    Discriminator = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true),
                    DeliveryArea_Latitude = table.Column<int>(nullable: true),
                    DeliveryArea_Longitude = table.Column<int>(nullable: true),
                    DeliveryArea_Radius = table.Column<int>(nullable: true),
                    Logistician_CompanyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.UniqueConstraint("AK_User_Login", x => x.Login);
                    table.ForeignKey(
                        name: "FK_User_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Companies_Logistician_CompanyId",
                        column: x => x.Logistician_CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Car",
                columns: table => new
                {
                    Number = table.Column<string>(maxLength: 12, nullable: false),
                    Model = table.Column<string>(nullable: true),
                    Volume = table.Column<float>(nullable: false),
                    Carrying = table.Column<float>(nullable: false),
                    DriverId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Car", x => x.Number);
                    table.ForeignKey(
                        name: "FK_Car_User_DriverId",
                        column: x => x.DriverId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Passport",
                columns: table => new
                {
                    ClientId = table.Column<int>(nullable: false),
                    Series = table.Column<string>(nullable: false),
                    Number = table.Column<string>(nullable: false),
                    Surname = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    Patronymic = table.Column<string>(nullable: true),
                    Sex = table.Column<string>(nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    BirthPlace_Latitude = table.Column<int>(nullable: true),
                    BirthPlace_Longitude = table.Column<int>(nullable: true),
                    BirthPlace_Index = table.Column<string>(nullable: true),
                    BirthPlace_Country = table.Column<string>(nullable: true),
                    BirthPlace_Region = table.Column<string>(nullable: true),
                    BirthPlace_City = table.Column<string>(nullable: true),
                    BirthPlace_Street = table.Column<string>(nullable: true),
                    BirthPlace_House = table.Column<string>(nullable: true),
                    BirthPlace_Flat = table.Column<string>(nullable: true),
                    BirthPlace_Addition = table.Column<string>(nullable: true),
                    IssuedBy = table.Column<string>(nullable: true),
                    IssuedDate = table.Column<DateTime>(nullable: false),
                    Code = table.Column<string>(maxLength: 7, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passport", x => x.ClientId);
                    table.ForeignKey(
                        name: "FK_Passport_User_ClientId",
                        column: x => x.ClientId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    CompanyId = table.Column<int>(nullable: false),
                    ClientId = table.Column<int>(nullable: false),
                    Mark = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => new { x.ClientId, x.CompanyId });
                    table.ForeignKey(
                        name: "FK_Ratings_User_ClientId",
                        column: x => x.ClientId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ratings_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(nullable: true),
                    DriverId = table.Column<int>(nullable: true),
                    CurrentStatus = table.Column<int>(nullable: false),
                    CurrentLatitude = table.Column<int>(nullable: false),
                    CurrentLongitude = table.Column<int>(nullable: false),
                    SendingAddress_Latitude = table.Column<int>(nullable: true),
                    SendingAddress_Longitude = table.Column<int>(nullable: true),
                    SendingAddress_Index = table.Column<string>(nullable: true),
                    SendingAddress_Country = table.Column<string>(nullable: true),
                    SendingAddress_Region = table.Column<string>(nullable: true),
                    SendingAddress_City = table.Column<string>(nullable: true),
                    SendingAddress_Street = table.Column<string>(nullable: true),
                    SendingAddress_House = table.Column<string>(nullable: true),
                    SendingAddress_Flat = table.Column<string>(nullable: true),
                    SendingAddress_Addition = table.Column<string>(nullable: true),
                    SendingDateTime = table.Column<DateTime>(nullable: false),
                    ReceivingAddress_Latitude = table.Column<int>(nullable: true),
                    ReceivingAddress_Longitude = table.Column<int>(nullable: true),
                    ReceivingAddress_Index = table.Column<string>(nullable: true),
                    ReceivingAddress_Country = table.Column<string>(nullable: true),
                    ReceivingAddress_Region = table.Column<string>(nullable: true),
                    ReceivingAddress_City = table.Column<string>(nullable: true),
                    ReceivingAddress_Street = table.Column<string>(nullable: true),
                    ReceivingAddress_House = table.Column<string>(nullable: true),
                    ReceivingAddress_Flat = table.Column<string>(nullable: true),
                    ReceivingAddress_Addition = table.Column<string>(nullable: true),
                    ReceivingDateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requests_User_ClientId",
                        column: x => x.ClientId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_User_DriverId",
                        column: x => x.DriverId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Good",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Weight = table.Column<float>(nullable: false),
                    Length = table.Column<float>(nullable: false),
                    Height = table.Column<float>(nullable: false),
                    Width = table.Column<float>(nullable: false),
                    IsFragile = table.Column<bool>(nullable: false),
                    RequestId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Good", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Good_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Car_DriverId",
                table: "Car",
                column: "DriverId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Good_RequestId",
                table: "Good",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_CompanyId",
                table: "Ratings",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ClientId",
                table: "Requests",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_DriverId",
                table: "Requests",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_User_CompanyId",
                table: "User",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Logistician_CompanyId",
                table: "User",
                column: "Logistician_CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Car");

            migrationBuilder.DropTable(
                name: "Good");

            migrationBuilder.DropTable(
                name: "Passport");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}

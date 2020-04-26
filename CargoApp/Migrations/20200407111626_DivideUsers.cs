using Microsoft.EntityFrameworkCore.Migrations;

namespace CargoApp.Migrations
{
    public partial class DivideUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Car_User_DriverId",
                table: "Car");

            migrationBuilder.DropForeignKey(
                name: "FK_Passport_User_ClientId",
                table: "Passport");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_User_ClientId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_User_ClientId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_User_DriverId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Companies_CompanyId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Companies_Logistician_CompanyId",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_User_Login",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_Logistician_CompanyId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Logistician_CompanyId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "User");

            migrationBuilder.DropColumn(
                name: "DeliveryArea_Latitude",
                table: "User");

            migrationBuilder.DropColumn(
                name: "DeliveryArea_Longitude",
                table: "User");

            migrationBuilder.DropColumn(
                name: "DeliveryArea_Radius",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Logisticians");

            migrationBuilder.RenameIndex(
                name: "IX_User_CompanyId",
                table: "Logisticians",
                newName: "IX_Logisticians_CompanyId");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Logisticians",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Logisticians",
                table: "Logisticians",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Logisticians_Login",
                table: "Logisticians",
                column: "Login");

            migrationBuilder.CreateTable(
                name: "UserRegData",
                columns: table => new
                {
                    Login = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Salt = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true, defaultValue: "Пользователь")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRegData", x => x.Login);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.UniqueConstraint("AK_Clients_Login", x => x.Login);
                    table.ForeignKey(
                        name: "FK_Clients_UserRegData_Login",
                        column: x => x.Login,
                        principalTable: "UserRegData",
                        principalColumn: "Login",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(nullable: false),
                    CompanyId = table.Column<int>(nullable: false),
                    DeliveryArea_Latitude = table.Column<int>(nullable: true),
                    DeliveryArea_Longitude = table.Column<int>(nullable: true),
                    DeliveryArea_Radius = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                    table.UniqueConstraint("AK_Drivers_Login", x => x.Login);
                    table.ForeignKey(
                        name: "FK_Drivers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Drivers_UserRegData_Login",
                        column: x => x.Login,
                        principalTable: "UserRegData",
                        principalColumn: "Login",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_CompanyId",
                table: "Drivers",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Car_Drivers_DriverId",
                table: "Car",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Logisticians_Companies_CompanyId",
                table: "Logisticians",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Logisticians_UserRegData_Login",
                table: "Logisticians",
                column: "Login",
                principalTable: "UserRegData",
                principalColumn: "Login",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Passport_Clients_ClientId",
                table: "Passport",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Clients_ClientId",
                table: "Ratings",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Clients_ClientId",
                table: "Requests",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Drivers_DriverId",
                table: "Requests",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Car_Drivers_DriverId",
                table: "Car");

            migrationBuilder.DropForeignKey(
                name: "FK_Logisticians_Companies_CompanyId",
                table: "Logisticians");

            migrationBuilder.DropForeignKey(
                name: "FK_Logisticians_UserRegData_Login",
                table: "Logisticians");

            migrationBuilder.DropForeignKey(
                name: "FK_Passport_Clients_ClientId",
                table: "Passport");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Clients_ClientId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Clients_ClientId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Drivers_DriverId",
                table: "Requests");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "UserRegData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Logisticians",
                table: "Logisticians");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Logisticians_Login",
                table: "Logisticians");

            migrationBuilder.RenameTable(
                name: "Logisticians",
                newName: "User");

            migrationBuilder.RenameIndex(
                name: "IX_Logisticians_CompanyId",
                table: "User",
                newName: "IX_User_CompanyId");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "User",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Logistician_CompanyId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "Пользователь");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DeliveryArea_Latitude",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryArea_Longitude",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryArea_Radius",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_User_Login",
                table: "User",
                column: "Login");

            migrationBuilder.CreateIndex(
                name: "IX_User_Logistician_CompanyId",
                table: "User",
                column: "Logistician_CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Car_User_DriverId",
                table: "Car",
                column: "DriverId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Passport_User_ClientId",
                table: "Passport",
                column: "ClientId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_User_ClientId",
                table: "Ratings",
                column: "ClientId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_User_ClientId",
                table: "Requests",
                column: "ClientId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_User_DriverId",
                table: "Requests",
                column: "DriverId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Companies_CompanyId",
                table: "User",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Companies_Logistician_CompanyId",
                table: "User",
                column: "Logistician_CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

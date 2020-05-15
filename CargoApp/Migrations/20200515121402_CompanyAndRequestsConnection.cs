using Microsoft.EntityFrameworkCore.Migrations;

namespace CargoApp.Migrations
{
    public partial class CompanyAndRequestsConnection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Requests",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Cars_Number",
                table: "Cars",
                column: "Number");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_CompanyId",
                table: "Requests",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Companies_CompanyId",
                table: "Requests",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Companies_CompanyId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_CompanyId",
                table: "Requests");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Cars_Number",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Requests");
        }
    }
}

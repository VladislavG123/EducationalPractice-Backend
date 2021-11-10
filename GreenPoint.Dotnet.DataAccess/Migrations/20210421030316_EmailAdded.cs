using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GreenPoint.Dotnet.DataAccess.Migrations
{
    public partial class EmailAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("90d8f2cf-99b5-478e-a54b-54101cb9eced"));

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreationDate", "Login", "Password" },
                values: new object[] { new Guid("ccbf4ecb-22bc-4579-af92-ad612cc04f21"), new DateTime(2021, 4, 21, 9, 3, 15, 156, DateTimeKind.Local).AddTicks(1887), "dev", "$2b$10$L03NivVaAFvIOaeU/JoOMurJxv8Ipw0FuXi1I8ukuzq/5JqOciU3O" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("ccbf4ecb-22bc-4579-af92-ad612cc04f21"));

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreationDate", "Login", "Password" },
                values: new object[] { new Guid("90d8f2cf-99b5-478e-a54b-54101cb9eced"), new DateTime(2021, 4, 20, 16, 28, 21, 347, DateTimeKind.Local).AddTicks(7077), "dev", "$2b$10$m1yU4Gf92XAN43ZfY5TOu.eqhAvLfpMVnPxgCPjxFOG1MhHQKjDEK" });
        }
    }
}

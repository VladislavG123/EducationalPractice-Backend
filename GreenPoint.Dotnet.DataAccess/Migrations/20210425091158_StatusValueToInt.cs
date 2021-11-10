using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GreenPoint.Dotnet.DataAccess.Migrations
{
    public partial class StatusValueToInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("dcf6df5e-d14a-4ee4-8608-a8fbefed87c2"));

            migrationBuilder.AlterColumn<int>(
                name: "MinValue",
                table: "Statuses",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "MaxValue",
                table: "Statuses",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreationDate", "Login", "Password" },
                values: new object[] { new Guid("fd6a8b23-1ffc-4b6c-98ad-b253f3d935f4"), new DateTime(2021, 4, 25, 15, 11, 57, 347, DateTimeKind.Local).AddTicks(8909), "dev", "$2b$10$S9PLvkDZ/PbtGeQIx/sZCOw00mu8Rm8mB/57MJ3x7ofdm5EhTfLxe" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("fd6a8b23-1ffc-4b6c-98ad-b253f3d935f4"));

            migrationBuilder.AlterColumn<long>(
                name: "MinValue",
                table: "Statuses",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "MaxValue",
                table: "Statuses",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreationDate", "Login", "Password" },
                values: new object[] { new Guid("dcf6df5e-d14a-4ee4-8608-a8fbefed87c2"), new DateTime(2021, 4, 25, 14, 34, 27, 151, DateTimeKind.Local).AddTicks(2730), "dev", "$2b$10$PpALSv3GMxTOlKQzPEJV0ORSpBbXz03lEvHKedtqiD1Z.I0KkVrJq" });
        }
    }
}

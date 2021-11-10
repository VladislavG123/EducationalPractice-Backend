using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GreenPoint.Dotnet.DataAccess.Migrations
{
    public partial class Status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("213f7bf1-4ede-4507-a38a-f383ff351092"));

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinValue = table.Column<long>(type: "bigint", nullable: false),
                    MaxValue = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreationDate", "Login", "Password" },
                values: new object[] { new Guid("dcf6df5e-d14a-4ee4-8608-a8fbefed87c2"), new DateTime(2021, 4, 25, 14, 34, 27, 151, DateTimeKind.Local).AddTicks(2730), "dev", "$2b$10$PpALSv3GMxTOlKQzPEJV0ORSpBbXz03lEvHKedtqiD1Z.I0KkVrJq" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("dcf6df5e-d14a-4ee4-8608-a8fbefed87c2"));

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreationDate", "Login", "Password" },
                values: new object[] { new Guid("213f7bf1-4ede-4507-a38a-f383ff351092"), new DateTime(2021, 4, 21, 19, 32, 35, 391, DateTimeKind.Local).AddTicks(6104), "dev", "$2b$10$8pJXXHLUhyM3TBGOXwk8PuPaWLIM/uj6Y1SJATy0YNgsenWnSdr7i" });
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GreenPoint.Dotnet.DataAccess.Migrations
{
    public partial class Migrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("ccbf4ecb-22bc-4579-af92-ad612cc04f21"));

            migrationBuilder.AlterColumn<Guid>(
                name: "CommentId",
                table: "Likes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreationDate", "Login", "Password" },
                values: new object[] { new Guid("213f7bf1-4ede-4507-a38a-f383ff351092"), new DateTime(2021, 4, 21, 19, 32, 35, 391, DateTimeKind.Local).AddTicks(6104), "dev", "$2b$10$8pJXXHLUhyM3TBGOXwk8PuPaWLIM/uj6Y1SJATy0YNgsenWnSdr7i" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: new Guid("213f7bf1-4ede-4507-a38a-f383ff351092"));

            migrationBuilder.AlterColumn<string>(
                name: "CommentId",
                table: "Likes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "CreationDate", "Login", "Password" },
                values: new object[] { new Guid("ccbf4ecb-22bc-4579-af92-ad612cc04f21"), new DateTime(2021, 4, 21, 9, 3, 15, 156, DateTimeKind.Local).AddTicks(1887), "dev", "$2b$10$L03NivVaAFvIOaeU/JoOMurJxv8Ipw0FuXi1I8ukuzq/5JqOciU3O" });
        }
    }
}

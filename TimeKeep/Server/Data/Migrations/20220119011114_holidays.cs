using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeKeep.Server.Data.Migrations
{
    public partial class holidays : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4fa8e07d-10e0-4775-9c1e-a3ffc95f339a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e2c996c6-92ac-4ea9-aede-c647a11620c8");

            migrationBuilder.CreateTable(
                name: "Holidays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holidays", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4cdede92-3f4d-4e7b-b5d5-f64465400a91", "d59b0b96-2487-4c4c-baf6-a909b3d23884", "General", "GENERAL" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d6105b31-27ae-460b-b4e8-709d707acd83", "ff8ce9a2-8198-4e66-985e-9fddb845726d", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Holidays");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4cdede92-3f4d-4e7b-b5d5-f64465400a91");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d6105b31-27ae-460b-b4e8-709d707acd83");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4fa8e07d-10e0-4775-9c1e-a3ffc95f339a", "d552c1b6-fb84-4c35-89ba-7e18a35ddb79", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e2c996c6-92ac-4ea9-aede-c647a11620c8", "739a2c71-624e-461d-9d92-9fb8ce458ae1", "General", "GENERAL" });
        }
    }
}

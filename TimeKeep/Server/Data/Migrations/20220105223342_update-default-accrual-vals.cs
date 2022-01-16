using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeKeep.Server.Data.Migrations
{
    public partial class updatedefaultaccrualvals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "337a4879-7792-46f8-a126-275ea34894e5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a78be3ce-98d8-4dd5-9a1b-a19dab7e2ef6");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "4fa8e07d-10e0-4775-9c1e-a3ffc95f339a", "d552c1b6-fb84-4c35-89ba-7e18a35ddb79", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e2c996c6-92ac-4ea9-aede-c647a11620c8", "739a2c71-624e-461d-9d92-9fb8ce458ae1", "General", "GENERAL" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4fa8e07d-10e0-4775-9c1e-a3ffc95f339a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e2c996c6-92ac-4ea9-aede-c647a11620c8");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "337a4879-7792-46f8-a126-275ea34894e5", "1a1c6d8f-323b-4387-a1c6-da3972ed4ed2", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a78be3ce-98d8-4dd5-9a1b-a19dab7e2ef6", "0036551d-e1d0-418d-84b0-c0a88c61277e", "General", "GENERAL" });
        }
    }
}

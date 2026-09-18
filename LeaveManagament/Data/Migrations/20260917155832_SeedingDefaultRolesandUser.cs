using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeaveManagament.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDefaultRolesandUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4d6877fe-c183-4d55-80d6-2e53cfc87273", "64e3d12e-c8a1-486f-8c33-9384e55d2951", "Supervisor", "SUPERVISOR" },
                    { "cbd2865a-8774-4514-927d-125b9b71ba84", "7d55f371-4acd-4d63-a1fd-420489b3be2d", "Employee", "EMPLOYEE" },
                    { "ee3106d3-f7fe-4a40-aa30-73bdbf8730f8", "89d38a6a-f9ec-48a7-8267-b1bf12111b54", "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "0bf3f41f-9b52-4233-918c-7e9d16177850", 0, "d0242dd8-335d-4363-8cea-f40600065882", "admin@localhost.com", true, false, null, "ADMIN@LOCALHOST.COM", "ADMIN@LOCALHOST.COM", "AQAAAAIAAYagAAAAEA9fL7HUHhLKG7f1iGrs5Y7dUjEUYbRzwxTAfFNCG5Vi85n5OI5yXHJQJKnQAG8Rkg==", null, false, "15b3fb44-a359-4ea3-9c82-2e4f6e7edf3a", false, "admin@localhost.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "ee3106d3-f7fe-4a40-aa30-73bdbf8730f8", "0bf3f41f-9b52-4233-918c-7e9d16177850" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d6877fe-c183-4d55-80d6-2e53cfc87273");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cbd2865a-8774-4514-927d-125b9b71ba84");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "ee3106d3-f7fe-4a40-aa30-73bdbf8730f8", "0bf3f41f-9b52-4233-918c-7e9d16177850" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ee3106d3-f7fe-4a40-aa30-73bdbf8730f8");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0bf3f41f-9b52-4233-918c-7e9d16177850");
        }
    }
}

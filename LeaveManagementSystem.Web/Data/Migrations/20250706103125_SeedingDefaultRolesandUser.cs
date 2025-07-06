using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeaveManagementSystem.Web.Data.Migrations
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
                    { "33b1258b-0dfa-44af-b3bd-b586beae7c9d", null, "Employee", "EMPLOYEE" },
                    { "6a773c09-6ffe-4202-bec1-d284be134155", null, "Supervisor", "SUPERVISOR" },
                    { "fc96c318-9a59-495e-a074-485e6335d878", null, "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "68d15c4b-b198-4a2d-ac2c-a9097a2b7859", 0, "543a9cd2-5fc9-4510-a4a7-0b4db4d39bea", "admin@localhost.om", true, false, null, "ADMIN@LOCALHOST.COM", "ADMIN@LOCALHOST.COM", "AQAAAAIAAYagAAAAENWfaBmGch20Wcq1A8SgvuGG0GMTLMf0gXUTMw8DXbzE1eDUlFv0QuQIKE2i9XkKGA==", null, false, "81813bc0-09c3-4c31-af19-f9e3c2209146", false, "admin@localhost.om" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "fc96c318-9a59-495e-a074-485e6335d878", "68d15c4b-b198-4a2d-ac2c-a9097a2b7859" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "33b1258b-0dfa-44af-b3bd-b586beae7c9d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6a773c09-6ffe-4202-bec1-d284be134155");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "fc96c318-9a59-495e-a074-485e6335d878", "68d15c4b-b198-4a2d-ac2c-a9097a2b7859" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fc96c318-9a59-495e-a074-485e6335d878");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "68d15c4b-b198-4a2d-ac2c-a9097a2b7859");
        }
    }
}

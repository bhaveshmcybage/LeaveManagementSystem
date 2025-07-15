using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "68d15c4b-b198-4a2d-ac2c-a9097a2b7859",
                columns: new[] { "ConcurrencyStamp", "DateOfBirth", "FirstName", "LastName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5a1ef776-f2f4-48ff-bd5e-5c9d747504a5", new DateOnly(1950, 12, 1), "Default", "Admin", "AQAAAAIAAYagAAAAEOfo8tNmqN/TeeRpuITTbDmiX6x4cAZF3Oeai1dD72gx0bYWIhtjXmtLj2pNhnZcEw==", "b3dab254-968f-4746-97d4-02871821179d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "68d15c4b-b198-4a2d-ac2c-a9097a2b7859",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "543a9cd2-5fc9-4510-a4a7-0b4db4d39bea", "AQAAAAIAAYagAAAAENWfaBmGch20Wcq1A8SgvuGG0GMTLMf0gXUTMw8DXbzE1eDUlFv0QuQIKE2i9XkKGA==", "81813bc0-09c3-4c31-af19-f9e3c2209146" });
        }
    }
}

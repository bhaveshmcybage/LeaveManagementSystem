using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "68d15c4b-b198-4a2d-ac2c-a9097a2b7859",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8121fe7a-c155-44d9-a6d1-a96afa809d4a", "AQAAAAIAAYagAAAAEI/jFsMFldZAFKoclGq7DHYFHNBUiHMZtEkxCujDkzJCZtJCu6HXCGxyjnyXuk+ofg==", "1e11dade-78d8-4234-a852-fffa81e59d7e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "68d15c4b-b198-4a2d-ac2c-a9097a2b7859",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "523b7c29-1d0f-469a-be15-68bc3c8b13cc", "AQAAAAIAAYagAAAAECvPUMmwq5va5IZIvszWTWgVMN1Mkog/NPN7pR3LZ6wJyBeSPuILtPD/BAzOWY4rhg==", "8c9f8edf-6924-4996-b161-a324ff5b41f0" });
        }
    }
}

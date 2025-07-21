using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Migrations
{
    /// <inheritdoc />
    public partial class UserCreatedToCreatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "verified",
                table: "Users",
                newName: "Verified");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Users",
                newName: "CreatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Verified",
                table: "Users",
                newName: "verified");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Users",
                newName: "Created");
        }
    }
}

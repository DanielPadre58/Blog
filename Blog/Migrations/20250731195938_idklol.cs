using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Migrations
{
    /// <inheritdoc />
    public partial class idklol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_CommentTypes_CommentTypeId",
                table: "Comments");

            migrationBuilder.DropTable(
                name: "CommentTypes");

            migrationBuilder.DropIndex(
                name: "IX_Comments_CommentTypeId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "CommentTypeId",
                table: "Comments");

            migrationBuilder.AddColumn<bool>(
                name: "isReply",
                table: "Comments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isReply",
                table: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "CommentTypeId",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CommentTypes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentTypes", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CommentTypeId",
                table: "Comments",
                column: "CommentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_CommentTypes_CommentTypeId",
                table: "Comments",
                column: "CommentTypeId",
                principalTable: "CommentTypes",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

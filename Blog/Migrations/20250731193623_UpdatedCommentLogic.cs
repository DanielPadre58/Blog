using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedCommentLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Comments",
                newName: "Typeid");

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
                name: "IX_Comments_Typeid",
                table: "Comments",
                column: "Typeid");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_CommentTypes_Typeid",
                table: "Comments",
                column: "Typeid",
                principalTable: "CommentTypes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_CommentTypes_Typeid",
                table: "Comments");

            migrationBuilder.DropTable(
                name: "CommentTypes");

            migrationBuilder.DropIndex(
                name: "IX_Comments_Typeid",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "Typeid",
                table: "Comments",
                newName: "Type");
        }
    }
}

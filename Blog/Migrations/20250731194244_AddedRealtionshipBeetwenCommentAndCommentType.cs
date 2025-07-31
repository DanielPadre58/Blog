using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Migrations
{
    /// <inheritdoc />
    public partial class AddedRealtionshipBeetwenCommentAndCommentType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_CommentTypes_Typeid",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "Typeid",
                table: "Comments",
                newName: "CommentTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_Typeid",
                table: "Comments",
                newName: "IX_Comments_CommentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_CommentTypes_CommentTypeId",
                table: "Comments",
                column: "CommentTypeId",
                principalTable: "CommentTypes",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_CommentTypes_CommentTypeId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "CommentTypeId",
                table: "Comments",
                newName: "Typeid");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_CommentTypeId",
                table: "Comments",
                newName: "IX_Comments_Typeid");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_CommentTypes_Typeid",
                table: "Comments",
                column: "Typeid",
                principalTable: "CommentTypes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

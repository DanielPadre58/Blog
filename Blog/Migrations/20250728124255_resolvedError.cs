using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Migrations
{
    /// <inheritdoc />
    public partial class resolvedError : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostLikes_Posts_LikedPostsId",
                table: "PostLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_PostLikes_Users_LikedByUsersId",
                table: "PostLikes");

            migrationBuilder.RenameColumn(
                name: "LikedPostsId",
                table: "PostLikes",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "LikedByUsersId",
                table: "PostLikes",
                newName: "PostId");

            migrationBuilder.RenameIndex(
                name: "IX_PostLikes_LikedPostsId",
                table: "PostLikes",
                newName: "IX_PostLikes_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostLikes_Posts_PostId",
                table: "PostLikes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostLikes_Users_UserId",
                table: "PostLikes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostLikes_Posts_PostId",
                table: "PostLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_PostLikes_Users_UserId",
                table: "PostLikes");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "PostLikes",
                newName: "LikedPostsId");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "PostLikes",
                newName: "LikedByUsersId");

            migrationBuilder.RenameIndex(
                name: "IX_PostLikes_UserId",
                table: "PostLikes",
                newName: "IX_PostLikes_LikedPostsId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostLikes_Posts_LikedPostsId",
                table: "PostLikes",
                column: "LikedPostsId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostLikes_Users_LikedByUsersId",
                table: "PostLikes",
                column: "LikedByUsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

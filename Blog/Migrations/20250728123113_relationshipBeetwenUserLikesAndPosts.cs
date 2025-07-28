using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blog.Migrations
{
    /// <inheritdoc />
    public partial class relationshipBeetwenUserLikesAndPosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostUser_Posts_LikedPostsId",
                table: "PostUser");

            migrationBuilder.DropForeignKey(
                name: "FK_PostUser_Users_UserId",
                table: "PostUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostUser",
                table: "PostUser");

            migrationBuilder.DropIndex(
                name: "IX_PostUser_UserId",
                table: "PostUser");

            migrationBuilder.RenameTable(
                name: "PostUser",
                newName: "PostLikes");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "PostLikes",
                newName: "LikedByUsersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostLikes",
                table: "PostLikes",
                columns: new[] { "LikedByUsersId", "LikedPostsId" });

            migrationBuilder.CreateIndex(
                name: "IX_PostLikes_LikedPostsId",
                table: "PostLikes",
                column: "LikedPostsId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostLikes_Posts_LikedPostsId",
                table: "PostLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_PostLikes_Users_LikedByUsersId",
                table: "PostLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostLikes",
                table: "PostLikes");

            migrationBuilder.DropIndex(
                name: "IX_PostLikes_LikedPostsId",
                table: "PostLikes");

            migrationBuilder.RenameTable(
                name: "PostLikes",
                newName: "PostUser");

            migrationBuilder.RenameColumn(
                name: "LikedByUsersId",
                table: "PostUser",
                newName: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostUser",
                table: "PostUser",
                columns: new[] { "LikedPostsId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_PostUser_UserId",
                table: "PostUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostUser_Posts_LikedPostsId",
                table: "PostUser",
                column: "LikedPostsId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostUser_Users_UserId",
                table: "PostUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

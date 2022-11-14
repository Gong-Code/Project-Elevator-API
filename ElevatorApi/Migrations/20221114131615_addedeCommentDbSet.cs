using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElevatorApi.Migrations
{
    public partial class addedeCommentDbSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentEntity_Errands_ErrandEntityId",
                table: "CommentEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentEntity",
                table: "CommentEntity");

            migrationBuilder.RenameTable(
                name: "CommentEntity",
                newName: "Comments");

            migrationBuilder.RenameIndex(
                name: "IX_CommentEntity_ErrandEntityId",
                table: "Comments",
                newName: "IX_Comments_ErrandEntityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Errands_ErrandEntityId",
                table: "Comments",
                column: "ErrandEntityId",
                principalTable: "Errands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Errands_ErrandEntityId",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "CommentEntity");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_ErrandEntityId",
                table: "CommentEntity",
                newName: "IX_CommentEntity_ErrandEntityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentEntity",
                table: "CommentEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentEntity_Errands_ErrandEntityId",
                table: "CommentEntity",
                column: "ErrandEntityId",
                principalTable: "Errands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

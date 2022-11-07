using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElevatorApi.Migrations
{
    public partial class addedUserName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByName",
                table: "Errands",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByName",
                table: "Elevators",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByName",
                table: "CommentEntity",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByName",
                table: "Errands");

            migrationBuilder.DropColumn(
                name: "CreatedByName",
                table: "Elevators");

            migrationBuilder.DropColumn(
                name: "CreatedByName",
                table: "CommentEntity");
        }
    }
}

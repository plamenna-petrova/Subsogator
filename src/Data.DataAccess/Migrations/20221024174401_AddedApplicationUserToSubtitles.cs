using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.DataAccess.Migrations
{
    public partial class AddedApplicationUserToSubtitles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Subtitles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subtitles_ApplicationUserId",
                table: "Subtitles",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subtitles_AspNetUsers_ApplicationUserId",
                table: "Subtitles",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subtitles_AspNetUsers_ApplicationUserId",
                table: "Subtitles");

            migrationBuilder.DropIndex(
                name: "IX_Subtitles_ApplicationUserId",
                table: "Subtitles");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Subtitles");
        }
    }
}

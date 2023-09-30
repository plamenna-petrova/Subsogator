using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.DataAccess.Migrations
{
    public partial class CascadeDeleteForSubtitlesComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Subtitles_SubtitlesId",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Subtitles_SubtitlesId",
                table: "Comments",
                column: "SubtitlesId",
                principalTable: "Subtitles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Subtitles_SubtitlesId",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Subtitles_SubtitlesId",
                table: "Comments",
                column: "SubtitlesId",
                principalTable: "Subtitles",
                principalColumn: "Id");
        }
    }
}

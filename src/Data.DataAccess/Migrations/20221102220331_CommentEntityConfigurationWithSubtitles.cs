using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.DataAccess.Migrations
{
    public partial class CommentEntityConfigurationWithSubtitles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubtitlesId",
                table: "Comments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_SubtitlesId",
                table: "Comments",
                column: "SubtitlesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Subtitles_SubtitlesId",
                table: "Comments",
                column: "SubtitlesId",
                principalTable: "Subtitles",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Subtitles_SubtitlesId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_SubtitlesId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "SubtitlesId",
                table: "Comments");
        }
    }
}

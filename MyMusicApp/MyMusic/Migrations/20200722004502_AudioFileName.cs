using Microsoft.EntityFrameworkCore.Migrations;

namespace MyMusic.Migrations
{
    public partial class AudioFileName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Playlists_AspNetUsers_ApplicationUserId",
                table: "Playlists");

            migrationBuilder.AddColumn<string>(
                name: "AudioFileName",
                table: "Songs",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApplicationUserViewModel",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserViewModel", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Playlists_ApplicationUserViewModel_ApplicationUserId",
                table: "Playlists",
                column: "ApplicationUserId",
                principalTable: "ApplicationUserViewModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Playlists_ApplicationUserViewModel_ApplicationUserId",
                table: "Playlists");

            migrationBuilder.DropTable(
                name: "ApplicationUserViewModel");

            migrationBuilder.DropColumn(
                name: "AudioFileName",
                table: "Songs");

            migrationBuilder.AddForeignKey(
                name: "FK_Playlists_AspNetUsers_ApplicationUserId",
                table: "Playlists",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

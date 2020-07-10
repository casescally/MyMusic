using Microsoft.EntityFrameworkCore.Migrations;

namespace MyMusic.Migrations
{
    public partial class PlaylistSongIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SongId",
                table: "Playlists");

            migrationBuilder.AddColumn<string>(
                name: "SongIds",
                table: "Playlists",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SongImages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SongId = table.Column<int>(nullable: false),
                    ImagePath = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SongImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SongImages_Songs_SongId",
                        column: x => x.SongId,
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SongImages_SongId",
                table: "SongImages",
                column: "SongId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SongImages");

            migrationBuilder.DropColumn(
                name: "SongIds",
                table: "Playlists");

            migrationBuilder.AddColumn<int>(
                name: "SongId",
                table: "Playlists",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

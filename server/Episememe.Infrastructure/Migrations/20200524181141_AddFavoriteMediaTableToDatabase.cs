using Microsoft.EntityFrameworkCore.Migrations;

namespace Episememe.Infrastructure.Migrations
{
    public partial class AddFavoriteMediaTableToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavoriteMedia",
                columns: table => new
                {
                    MediaInstanceId = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteMedia", x => new { x.MediaInstanceId, x.UserId });
                    table.ForeignKey(
                        name: "FK_FavoriteMedia_MediaInstances_MediaInstanceId",
                        column: x => x.MediaInstanceId,
                        principalTable: "MediaInstances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavoriteMedia");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Episememe.Infrastructure.Migrations
{
    public partial class AddTagConnectionsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TagConnections",
                columns: table => new
                {
                    AncestorId = table.Column<int>(nullable: false),
                    SuccessorId = table.Column<int>(nullable: false),
                    Depth = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagConnections", x => new { x.SuccessorId, x.AncestorId, x.Depth });
                    table.ForeignKey(
                        name: "FK_TagConnections_Tags_AncestorId",
                        column: x => x.AncestorId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TagConnections_Tags_SuccessorId",
                        column: x => x.SuccessorId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TagConnections_AncestorId",
                table: "TagConnections",
                column: "AncestorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TagConnections");
        }
    }
}

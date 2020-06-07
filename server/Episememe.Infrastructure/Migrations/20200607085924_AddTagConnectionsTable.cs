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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EntryEdgeId = table.Column<int>(nullable: false),
                    DirectEdgeId = table.Column<int>(nullable: false),
                    ExitEdgeId = table.Column<int>(nullable: false),
                    Successor = table.Column<int>(nullable: false),
                    Ancestor = table.Column<int>(nullable: false),
                    Hops = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagConnections", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TagConnections");
        }
    }
}

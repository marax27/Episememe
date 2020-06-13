using Microsoft.EntityFrameworkCore.Migrations;

namespace Episememe.Infrastructure.Migrations
{
    public partial class AddUnidirectionalOneToManyRelationshipBetweenTagAndTagConnection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NEW_TagConnections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EntryEdgeId = table.Column<int>(nullable: false),
                    DirectEdgeId = table.Column<int>(nullable: false),
                    ExitEdgeId = table.Column<int>(nullable: false),
                    SuccessorId = table.Column<int>(nullable: false),
                    AncestorId = table.Column<int>(nullable: false),
                    Hops = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagConnections", x => x.Id);
                    table.ForeignKey(name: "FK_TagConnections_Tags_SuccessorId",
                        column: x => x.SuccessorId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(name: "FK_TagConnections_Tags_AncestorId",
                        column: x => x.AncestorId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql("INSERT INTO NEW_TagConnections SELECT * FROM TagConnections;");
            migrationBuilder.Sql("PRAGMA foreign_keys=\"0\"", true);
            migrationBuilder.Sql("DROP TABLE TagConnections", true);
            migrationBuilder.Sql("ALTER TABLE NEW_TagConnections RENAME TO TagConnections", true);
            migrationBuilder.Sql("PRAGMA foreign_keys=\"1\"", true);

            migrationBuilder.CreateIndex(
                name: "IX_TagConnections_AncestorId",
                table: "TagConnections",
                column: "AncestorId");

            migrationBuilder.CreateIndex(
                name: "IX_TagConnections_SuccessorId",
                table: "TagConnections",
                column: "SuccessorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "NEW_TagConnections",
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

            migrationBuilder.DropIndex(
                name: "IX_TagConnections_AncestorId",
                table: "TagConnections");

            migrationBuilder.DropIndex(
                name: "IX_TagConnections_SuccessorId",
                table: "TagConnections");

            migrationBuilder.Sql("INSERT INTO NEW_TagConnections SELECT * FROM TagConnections;");
            migrationBuilder.Sql("PRAGMA foreign_keys=\"0\"", true);
            migrationBuilder.Sql("DROP TABLE TagConnections", true);
            migrationBuilder.Sql("ALTER TABLE NEW_TagConnections RENAME TO TagConnections", true);
            migrationBuilder.Sql("PRAGMA foreign_keys=\"1\"", true);
        }
    }
}

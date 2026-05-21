using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LavenderFlow_API.Migrations
{
    /// <inheritdoc />
    public partial class AddListItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkspaceId1",
                table: "Boards",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ListItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    BoardId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListItems_Boards_BoardId",
                        column: x => x.BoardId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Boards_WorkspaceId1",
                table: "Boards",
                column: "WorkspaceId1");

            migrationBuilder.CreateIndex(
                name: "IX_ListItems_BoardId",
                table: "ListItems",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_Workspaces_WorkspaceId1",
                table: "Boards",
                column: "WorkspaceId1",
                principalTable: "Workspaces",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boards_Workspaces_WorkspaceId1",
                table: "Boards");

            migrationBuilder.DropTable(
                name: "ListItems");

            migrationBuilder.DropIndex(
                name: "IX_Boards_WorkspaceId1",
                table: "Boards");

            migrationBuilder.DropColumn(
                name: "WorkspaceId1",
                table: "Boards");
        }
    }
}

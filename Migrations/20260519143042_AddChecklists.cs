using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LavenderFlow_API.Migrations
{
    /// <inheritdoc />
    public partial class AddChecklists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BoardId1",
                table: "ListItems",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ListItemId1",
                table: "Cards",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Checklists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CardId = table.Column<int>(type: "integer", nullable: false),
                    CardId1 = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checklists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Checklists_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Checklists_Cards_CardId1",
                        column: x => x.CardId1,
                        principalTable: "Cards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChecklistItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Finished = table.Column<bool>(type: "boolean", nullable: false),
                    ChecklistId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChecklistItems_Checklists_ChecklistId",
                        column: x => x.ChecklistId,
                        principalTable: "Checklists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ListItems_BoardId1",
                table: "ListItems",
                column: "BoardId1");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_ListItemId1",
                table: "Cards",
                column: "ListItemId1");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistItems_ChecklistId",
                table: "ChecklistItems",
                column: "ChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_Checklists_CardId",
                table: "Checklists",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_Checklists_CardId1",
                table: "Checklists",
                column: "CardId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_ListItems_ListItemId1",
                table: "Cards",
                column: "ListItemId1",
                principalTable: "ListItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ListItems_Boards_BoardId1",
                table: "ListItems",
                column: "BoardId1",
                principalTable: "Boards",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_ListItems_ListItemId1",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_ListItems_Boards_BoardId1",
                table: "ListItems");

            migrationBuilder.DropTable(
                name: "ChecklistItems");

            migrationBuilder.DropTable(
                name: "Checklists");

            migrationBuilder.DropIndex(
                name: "IX_ListItems_BoardId1",
                table: "ListItems");

            migrationBuilder.DropIndex(
                name: "IX_Cards_ListItemId1",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "BoardId1",
                table: "ListItems");

            migrationBuilder.DropColumn(
                name: "ListItemId1",
                table: "Cards");
        }
    }
}

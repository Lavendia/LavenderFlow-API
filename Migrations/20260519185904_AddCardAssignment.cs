using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LavenderFlow_API.Migrations
{
    /// <inheritdoc />
    public partial class AddCardAssignment : Migration
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
                name: "CardAssignments",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CardId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardAssignments", x => new { x.UserId, x.CardId });
                    table.ForeignKey(
                        name: "FK_CardAssignments_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardAssignments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
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
                name: "IX_CardAssignments_CardId",
                table: "CardAssignments",
                column: "CardId");

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
                name: "CardAssignments");

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

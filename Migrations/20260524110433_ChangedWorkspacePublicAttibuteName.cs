using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LavenderFlow_API.Migrations
{
    /// <inheritdoc />
    public partial class ChangedWorkspacePublicAttibuteName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Public",
                table: "Workspaces",
                newName: "IsPublic");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPublic",
                table: "Workspaces",
                newName: "Public");
        }
    }
}

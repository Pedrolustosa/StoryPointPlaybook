using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryPointPlaybook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate_III : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DisplayName",
                table: "Users",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "DisplayName");
        }
    }
}

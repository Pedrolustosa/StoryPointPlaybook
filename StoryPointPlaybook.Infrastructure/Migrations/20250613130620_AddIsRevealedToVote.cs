using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryPointPlaybook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIsRevealedToVote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRevealed",
                table: "Votes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRevealed",
                table: "Votes");
        }
    }
}

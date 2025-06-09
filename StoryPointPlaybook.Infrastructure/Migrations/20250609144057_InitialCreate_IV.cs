using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryPointPlaybook.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate_IV : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CurrentStoryId",
                table: "Rooms",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentStoryId",
                table: "Rooms");
        }
    }
}

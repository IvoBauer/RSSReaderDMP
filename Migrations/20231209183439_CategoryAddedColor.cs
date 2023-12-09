using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RSSReader.Migrations
{
    /// <inheritdoc />
    public partial class CategoryAddedColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "FeedCategories",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "FeedCategories");
        }
    }
}

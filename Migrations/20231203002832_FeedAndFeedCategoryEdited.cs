using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RSSReader.Migrations
{
    /// <inheritdoc />
    public partial class FeedAndFeedCategoryEdited : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FeedCategoryId",
                table: "Feeds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FeedCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedCategory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Feeds_FeedCategoryId",
                table: "Feeds",
                column: "FeedCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feeds_FeedCategory_FeedCategoryId",
                table: "Feeds",
                column: "FeedCategoryId",
                principalTable: "FeedCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feeds_FeedCategory_FeedCategoryId",
                table: "Feeds");

            migrationBuilder.DropTable(
                name: "FeedCategory");

            migrationBuilder.DropIndex(
                name: "IX_Feeds_FeedCategoryId",
                table: "Feeds");

            migrationBuilder.DropColumn(
                name: "FeedCategoryId",
                table: "Feeds");
        }
    }
}

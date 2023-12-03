using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RSSReader.Migrations
{
    /// <inheritdoc />
    public partial class DBUpdate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feeds_FeedCategory_FeedCategoryId",
                table: "Feeds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeedCategory",
                table: "FeedCategory");

            migrationBuilder.RenameTable(
                name: "FeedCategory",
                newName: "FeedCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeedCategories",
                table: "FeedCategories",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "FeedCategoryRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeedCategoryId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedCategoryRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedCategoryRecords_FeedCategories_FeedCategoryId",
                        column: x => x.FeedCategoryId,
                        principalTable: "FeedCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeedCategoryRecords_FeedCategoryId",
                table: "FeedCategoryRecords",
                column: "FeedCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feeds_FeedCategories_FeedCategoryId",
                table: "Feeds",
                column: "FeedCategoryId",
                principalTable: "FeedCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feeds_FeedCategories_FeedCategoryId",
                table: "Feeds");

            migrationBuilder.DropTable(
                name: "FeedCategoryRecords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeedCategories",
                table: "FeedCategories");

            migrationBuilder.RenameTable(
                name: "FeedCategories",
                newName: "FeedCategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeedCategory",
                table: "FeedCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Feeds_FeedCategory_FeedCategoryId",
                table: "Feeds",
                column: "FeedCategoryId",
                principalTable: "FeedCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

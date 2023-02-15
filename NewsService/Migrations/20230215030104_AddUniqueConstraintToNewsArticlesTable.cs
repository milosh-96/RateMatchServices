using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsService.Migrations
{
    public partial class AddUniqueConstraintToNewsArticlesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Articles_Title_PublishedAt_Source",
                table: "Articles",
                columns: new[] { "Title", "PublishedAt", "Source" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Articles_Title_PublishedAt_Source",
                table: "Articles");
        }
    }
}

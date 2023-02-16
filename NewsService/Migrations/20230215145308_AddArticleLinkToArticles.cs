using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsService.Migrations
{
    public partial class AddArticleLinkToArticles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Articles_Title_PublishedAt_Source",
                table: "Articles");

            migrationBuilder.AddColumn<string>(
                name: "ArticleLink",
                table: "Articles",
                type: "varchar(256)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_Title_PublishedAt_Source_ArticleLink",
                table: "Articles",
                columns: new[] { "Title", "PublishedAt", "Source", "ArticleLink" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Articles_Title_PublishedAt_Source_ArticleLink",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ArticleLink",
                table: "Articles");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_Title_PublishedAt_Source",
                table: "Articles",
                columns: new[] { "Title", "PublishedAt", "Source" },
                unique: true);
        }
    }
}

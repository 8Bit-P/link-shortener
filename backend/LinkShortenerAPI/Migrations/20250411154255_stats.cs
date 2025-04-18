using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkShortenerAPI.Migrations
{
    /// <inheritdoc />
    public partial class stats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IpAddress",
                table: "UrlAccessLogs",
                newName: "Referer");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "UrlAccessLogs",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "UrlAccessLogs");

            migrationBuilder.RenameColumn(
                name: "Referer",
                table: "UrlAccessLogs",
                newName: "IpAddress");
        }
    }
}

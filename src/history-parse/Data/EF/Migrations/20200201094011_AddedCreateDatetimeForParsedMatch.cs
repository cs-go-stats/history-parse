using Microsoft.EntityFrameworkCore.Migrations;

namespace CSGOStats.Services.HistoryParse.Data.EF.Migrations
{
    public partial class AddedCreateDatetimeForParsedMatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedAt",
                schema: "HistoryParse",
                table: "ParsedMatch",
                nullable: false,
                defaultValue: "2020-02-01T09:40:11.2382662Z (ISO)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "HistoryParse",
                table: "ParsedMatch");
        }
    }
}

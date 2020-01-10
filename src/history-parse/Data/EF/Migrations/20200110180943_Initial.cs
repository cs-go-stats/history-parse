using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CSGOStats.Services.HistoryParse.Data.EF.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "HistoryParse");

            migrationBuilder.CreateTable(
                name: "ParsedMatch",
                schema: "HistoryParse",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Version = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParsedMatch", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParsedMatch",
                schema: "HistoryParse");
        }
    }
}

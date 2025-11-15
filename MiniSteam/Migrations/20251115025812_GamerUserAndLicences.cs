using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniSteam.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class GamerUserAndLicences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Classification",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Classification",
                table: "Games");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AestheticWordle.Api.Migrations
{
    /// <inheritdoc />
    public partial class Seconds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Seconds",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Seconds",
                table: "Games");
        }
    }
}

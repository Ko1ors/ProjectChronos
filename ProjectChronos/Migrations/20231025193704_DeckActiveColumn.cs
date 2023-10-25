using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectChronos.Migrations
{
    /// <inheritdoc />
    public partial class DeckActiveColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "DeckCard",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "DeckCard");
        }
    }
}

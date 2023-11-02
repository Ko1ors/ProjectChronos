using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectChronos.Migrations
{
    /// <inheritdoc />
    public partial class CreatedPacksInternalId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InternalId",
                table: "CreatedPacks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InternalId",
                table: "CreatedPacks");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectChronos.Migrations
{
    /// <inheritdoc />
    public partial class AddedOpponentUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OpponentUserId",
                table: "Opponents",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Opponents_OpponentUserId",
                table: "Opponents",
                column: "OpponentUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Opponents_AspNetUsers_OpponentUserId",
                table: "Opponents",
                column: "OpponentUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Opponents_AspNetUsers_OpponentUserId",
                table: "Opponents");

            migrationBuilder.DropIndex(
                name: "IX_Opponents_OpponentUserId",
                table: "Opponents");

            migrationBuilder.DropColumn(
                name: "OpponentUserId",
                table: "Opponents");
        }
    }
}

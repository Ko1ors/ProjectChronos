using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectChronos.Migrations
{
    /// <inheritdoc />
    public partial class AddedOpponentUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "OpponentUser",
                columns: table => new
                {
                    OpponentUsersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OpponentsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpponentUser", x => new { x.OpponentUsersId, x.OpponentsId });
                    table.ForeignKey(
                        name: "FK_OpponentUser_AspNetUsers_OpponentUsersId",
                        column: x => x.OpponentUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OpponentUser_Opponents_OpponentsId",
                        column: x => x.OpponentsId,
                        principalTable: "Opponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OpponentUser_OpponentsId",
                table: "OpponentUser",
                column: "OpponentsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OpponentUser");

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
    }
}

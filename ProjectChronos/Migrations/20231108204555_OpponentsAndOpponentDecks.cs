using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectChronos.Migrations
{
    /// <inheritdoc />
    public partial class OpponentsAndOpponentDecks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "UserDecks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Opponents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    OpponentDeckId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Opponents_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Opponents_UserDecks_OpponentDeckId",
                        column: x => x.OpponentDeckId,
                        principalTable: "UserDecks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserOpponent",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OpponentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOpponent", x => new { x.UserId, x.OpponentId });
                    table.ForeignKey(
                        name: "FK_UserOpponent_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserOpponent_Opponents_OpponentId",
                        column: x => x.OpponentId,
                        principalTable: "Opponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Opponents_OpponentDeckId",
                table: "Opponents",
                column: "OpponentDeckId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Opponents_UserId",
                table: "Opponents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOpponent_OpponentId",
                table: "UserOpponent",
                column: "OpponentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserOpponent");

            migrationBuilder.DropTable(
                name: "Opponents");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "UserDecks");
        }
    }
}

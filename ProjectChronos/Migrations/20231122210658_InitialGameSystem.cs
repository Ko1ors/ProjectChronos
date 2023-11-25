using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectChronos.Migrations
{
    /// <inheritdoc />
    public partial class InitialGameSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MatchInstance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpponentId = table.Column<int>(type: "int", nullable: false),
                    Result = table.Column<int>(type: "int", nullable: false),
                    SystemVersion = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserDeckSnapshotId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchInstance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchInstance_UserDecks_UserDeckSnapshotId",
                        column: x => x.UserDeckSnapshotId,
                        principalTable: "UserDecks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchDrawnCard",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    MatchDrawTurnId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchDrawnCard", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MatchTurn",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Index = table.Column<int>(type: "int", nullable: false),
                    MatchInstanceId = table.Column<int>(type: "int", nullable: false),
                    IsUserTurn = table.Column<bool>(type: "bit", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttackCardId = table.Column<int>(type: "int", nullable: true),
                    TargetCardId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchTurn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchTurn_MatchDrawnCard_AttackCardId",
                        column: x => x.AttackCardId,
                        principalTable: "MatchDrawnCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatchTurn_MatchDrawnCard_TargetCardId",
                        column: x => x.TargetCardId,
                        principalTable: "MatchDrawnCard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatchTurn_MatchInstance_MatchInstanceId",
                        column: x => x.MatchInstanceId,
                        principalTable: "MatchInstance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatchDrawnCard_MatchDrawTurnId",
                table: "MatchDrawnCard",
                column: "MatchDrawTurnId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchInstance_UserDeckSnapshotId",
                table: "MatchInstance",
                column: "UserDeckSnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchTurn_AttackCardId",
                table: "MatchTurn",
                column: "AttackCardId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchTurn_MatchInstanceId",
                table: "MatchTurn",
                column: "MatchInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchTurn_TargetCardId",
                table: "MatchTurn",
                column: "TargetCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchDrawnCard_MatchTurn_MatchDrawTurnId",
                table: "MatchDrawnCard",
                column: "MatchDrawTurnId",
                principalTable: "MatchTurn",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchDrawnCard_MatchTurn_MatchDrawTurnId",
                table: "MatchDrawnCard");

            migrationBuilder.DropTable(
                name: "MatchTurn");

            migrationBuilder.DropTable(
                name: "MatchDrawnCard");

            migrationBuilder.DropTable(
                name: "MatchInstance");
        }
    }
}

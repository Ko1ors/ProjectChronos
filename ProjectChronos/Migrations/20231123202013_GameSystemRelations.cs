using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectChronos.Migrations
{
    /// <inheritdoc />
    public partial class GameSystemRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchInstance_UserDecks_UserDeckSnapshotId",
                table: "MatchInstance");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchTurn_MatchInstance_MatchInstanceId",
                table: "MatchTurn");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MatchInstance",
                table: "MatchInstance");

            migrationBuilder.RenameTable(
                name: "MatchInstance",
                newName: "Matches");

            migrationBuilder.RenameIndex(
                name: "IX_MatchInstance_UserDeckSnapshotId",
                table: "Matches",
                newName: "IX_Matches_UserDeckSnapshotId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Matches",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Matches",
                table: "Matches",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_OpponentId",
                table: "Matches",
                column: "OpponentId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_UserId",
                table: "Matches",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_AspNetUsers_UserId",
                table: "Matches",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Opponents_OpponentId",
                table: "Matches",
                column: "OpponentId",
                principalTable: "Opponents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_UserDecks_UserDeckSnapshotId",
                table: "Matches",
                column: "UserDeckSnapshotId",
                principalTable: "UserDecks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchTurn_Matches_MatchInstanceId",
                table: "MatchTurn",
                column: "MatchInstanceId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_AspNetUsers_UserId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Opponents_OpponentId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_UserDecks_UserDeckSnapshotId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchTurn_Matches_MatchInstanceId",
                table: "MatchTurn");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Matches",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_OpponentId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_UserId",
                table: "Matches");

            migrationBuilder.RenameTable(
                name: "Matches",
                newName: "MatchInstance");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_UserDeckSnapshotId",
                table: "MatchInstance",
                newName: "IX_MatchInstance_UserDeckSnapshotId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "MatchInstance",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MatchInstance",
                table: "MatchInstance",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchInstance_UserDecks_UserDeckSnapshotId",
                table: "MatchInstance",
                column: "UserDeckSnapshotId",
                principalTable: "UserDecks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchTurn_MatchInstance_MatchInstanceId",
                table: "MatchTurn",
                column: "MatchInstanceId",
                principalTable: "MatchInstance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

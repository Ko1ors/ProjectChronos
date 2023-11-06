using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectChronos.Migrations
{
    /// <inheritdoc />
    public partial class CardDecksFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeckCard_UserDeck_UserDeckId",
                table: "DeckCard");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDeck_AspNetUsers_UserId",
                table: "UserDeck");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserDeck",
                table: "UserDeck");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeckCard",
                table: "DeckCard");

            migrationBuilder.RenameTable(
                name: "UserDeck",
                newName: "UserDecks");

            migrationBuilder.RenameTable(
                name: "DeckCard",
                newName: "DeckCards");

            migrationBuilder.RenameIndex(
                name: "IX_UserDeck_UserId",
                table: "UserDecks",
                newName: "IX_UserDecks_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_DeckCard_UserDeckId",
                table: "DeckCards",
                newName: "IX_DeckCards_UserDeckId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserDecks",
                table: "UserDecks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeckCards",
                table: "DeckCards",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeckCards_UserDecks_UserDeckId",
                table: "DeckCards",
                column: "UserDeckId",
                principalTable: "UserDecks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDecks_AspNetUsers_UserId",
                table: "UserDecks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeckCards_UserDecks_UserDeckId",
                table: "DeckCards");

            migrationBuilder.DropForeignKey(
                name: "FK_UserDecks_AspNetUsers_UserId",
                table: "UserDecks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserDecks",
                table: "UserDecks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeckCards",
                table: "DeckCards");

            migrationBuilder.RenameTable(
                name: "UserDecks",
                newName: "UserDeck");

            migrationBuilder.RenameTable(
                name: "DeckCards",
                newName: "DeckCard");

            migrationBuilder.RenameIndex(
                name: "IX_UserDecks_UserId",
                table: "UserDeck",
                newName: "IX_UserDeck_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_DeckCards_UserDeckId",
                table: "DeckCard",
                newName: "IX_DeckCard_UserDeckId");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "DeckCard",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserDeck",
                table: "UserDeck",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeckCard",
                table: "DeckCard",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeckCard_UserDeck_UserDeckId",
                table: "DeckCard",
                column: "UserDeckId",
                principalTable: "UserDeck",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDeck_AspNetUsers_UserId",
                table: "UserDeck",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

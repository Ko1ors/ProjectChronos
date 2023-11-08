using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectChronos.Migrations
{
    /// <inheritdoc />
    public partial class UserDeckNullableUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDecks_AspNetUsers_UserId",
                table: "UserDecks");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserDecks",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDecks_AspNetUsers_UserId",
                table: "UserDecks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDecks_AspNetUsers_UserId",
                table: "UserDecks");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserDecks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserDecks_AspNetUsers_UserId",
                table: "UserDecks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectChronos.Migrations
{
    /// <inheritdoc />
    public partial class InitialCardPacks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CardPackRewardTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TokenId = table.Column<int>(type: "int", nullable: false),
                    QuantityPerReward = table.Column<int>(type: "int", nullable: false),
                    TotalRewards = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardPackRewardTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardPackTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    RewardsPerPack = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardPackTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardPackRewardTemplateCardPackTemplate",
                columns: table => new
                {
                    CardPackTemplatesId = table.Column<int>(type: "int", nullable: false),
                    RewardTemplatesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardPackRewardTemplateCardPackTemplate", x => new { x.CardPackTemplatesId, x.RewardTemplatesId });
                    table.ForeignKey(
                        name: "FK_CardPackRewardTemplateCardPackTemplate_CardPackRewardTemplates_RewardTemplatesId",
                        column: x => x.RewardTemplatesId,
                        principalTable: "CardPackRewardTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardPackRewardTemplateCardPackTemplate_CardPackTemplates_CardPackTemplatesId",
                        column: x => x.CardPackTemplatesId,
                        principalTable: "CardPackTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreatedPacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardPackTemplateId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    QuantityRemaining = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatedPacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreatedPacks_CardPackTemplates_CardPackTemplateId",
                        column: x => x.CardPackTemplateId,
                        principalTable: "CardPackTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardPackRewardTemplateCardPackTemplate_RewardTemplatesId",
                table: "CardPackRewardTemplateCardPackTemplate",
                column: "RewardTemplatesId");

            migrationBuilder.CreateIndex(
                name: "IX_CreatedPacks_CardPackTemplateId",
                table: "CreatedPacks",
                column: "CardPackTemplateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardPackRewardTemplateCardPackTemplate");

            migrationBuilder.DropTable(
                name: "CreatedPacks");

            migrationBuilder.DropTable(
                name: "CardPackRewardTemplates");

            migrationBuilder.DropTable(
                name: "CardPackTemplates");
        }
    }
}

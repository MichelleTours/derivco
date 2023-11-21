using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Derivco.Casino.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PayoutMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BetOption = table.Column<int>(type: "INTEGER", nullable: false),
                    SpinValue = table.Column<int>(type: "INTEGER", nullable: false),
                    Multiplier = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayoutMaps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rounds",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CorrelationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DateStarted = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateBetsPlaced = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateWheelSpin = table.Column<DateTime>(type: "TEXT", nullable: true),
                    SpinValue = table.Column<int>(type: "INTEGER", nullable: true),
                    DateOfPayout = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsInPlay = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rounds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CorrelationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoundCorrelationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoundId = table.Column<long>(type: "INTEGER", nullable: false),
                    Option = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<decimal>(type: "TEXT", nullable: false),
                    HasPayout = table.Column<bool>(type: "INTEGER", nullable: false),
                    PayoutValue = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bets_Rounds_RoundId",
                        column: x => x.RoundId,
                        principalTable: "Rounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bets_RoundId",
                table: "Bets",
                column: "RoundId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bets");

            migrationBuilder.DropTable(
                name: "PayoutMaps");

            migrationBuilder.DropTable(
                name: "Rounds");
        }
    }
}

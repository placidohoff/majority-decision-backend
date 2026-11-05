using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajorityDecision.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fighters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Suffix = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fighters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FighterAId = table.Column<int>(type: "int", nullable: false),
                    FighterBId = table.Column<int>(type: "int", nullable: false),
                    ScheduledRounds = table.Column<int>(type: "int", nullable: false),
                    WeightClass = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    MeetingNumber = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: true),
                    Venue = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Referee = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fights", x => x.Id);
                    table.CheckConstraint("CK_Fight_DifferentFighters", "[FighterAId] <> [FighterBId]");
                    table.CheckConstraint("CK_Fight_MeetingNumber", "[MeetingNumber] >= 1");
                    table.CheckConstraint("CK_Fight_ScheduledRounds", "[ScheduledRounds] BETWEEN 1 AND 15");
                    table.ForeignKey(
                        name: "FK_Fights_Fighters_FighterAId",
                        column: x => x.FighterAId,
                        principalTable: "Fighters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fights_Fighters_FighterBId",
                        column: x => x.FighterBId,
                        principalTable: "Fighters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Scorecards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FightId = table.Column<int>(type: "int", nullable: false),
                    IsOfficial = table.Column<bool>(type: "bit", nullable: false),
                    JudgeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scorecards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scorecards_Fights_FightId",
                        column: x => x.FightId,
                        principalTable: "Fights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoundScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScorecardId = table.Column<int>(type: "int", nullable: false),
                    RoundNumber = table.Column<int>(type: "int", nullable: false),
                    FighterAScore = table.Column<int>(type: "int", nullable: false),
                    FighterBScore = table.Column<int>(type: "int", nullable: false),
                    FighterAKnockdowns = table.Column<int>(type: "int", nullable: false),
                    FighterBKnockdowns = table.Column<int>(type: "int", nullable: false),
                    FighterADeductions = table.Column<int>(type: "int", nullable: false),
                    FighterBDeductions = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoundScores", x => x.Id);
                    table.CheckConstraint("CK_RoundScore_RoundNumber", "[RoundNumber] >= 1");
                    table.ForeignKey(
                        name: "FK_RoundScores_Scorecards_ScorecardId",
                        column: x => x.ScorecardId,
                        principalTable: "Scorecards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fighters_LastName_FirstName",
                table: "Fighters",
                columns: new[] { "LastName", "FirstName" });

            migrationBuilder.CreateIndex(
                name: "IX_Fights_FighterAId_FighterBId_MeetingNumber",
                table: "Fights",
                columns: new[] { "FighterAId", "FighterBId", "MeetingNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fights_FighterBId",
                table: "Fights",
                column: "FighterBId");

            migrationBuilder.CreateIndex(
                name: "IX_RoundScores_ScorecardId_RoundNumber",
                table: "RoundScores",
                columns: new[] { "ScorecardId", "RoundNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scorecards_FightId",
                table: "Scorecards",
                column: "FightId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoundScores");

            migrationBuilder.DropTable(
                name: "Scorecards");

            migrationBuilder.DropTable(
                name: "Fights");

            migrationBuilder.DropTable(
                name: "Fighters");
        }
    }
}

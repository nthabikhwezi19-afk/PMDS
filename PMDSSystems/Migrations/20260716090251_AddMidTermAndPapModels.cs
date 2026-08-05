using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMDSSystems.Migrations
{
    /// <inheritdoc />
    public partial class AddMidTermAndPapModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgeGroup",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "AppointmentDateInCurrentRank",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "AppointmentDateInDCS",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CurrentRankPostLevel",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PostDesignation",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "RelatedOSDDescription",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "SupervisorRankPostLevel",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "SupervisorSurnameInitials",
                table: "Employees");

            migrationBuilder.AlterColumn<string>(
                name: "PersalNumber",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(9)",
                oldMaxLength: 9);

            migrationBuilder.AlterColumn<string>(
                name: "IdentificationNumber",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13);

            migrationBuilder.CreateTable(
                name: "MidTermReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReviewPeriod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasDispute = table.Column<bool>(type: "bit", nullable: false),
                    DisputedKraNumbers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HigherManagerDecision = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuperviseeSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuperviseeSignDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SupervisorSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupervisorSignDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HigherManagerSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HigherManagerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HigherManagerSignDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MidTermReviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonalAssistancePlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Post = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersalNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupervisorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RemedialSteps = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Month1Progress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Month2Progress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Month3Progress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Month4Progress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Month5Progress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Month6Progress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPerformanceUpToStandard = table.Column<bool>(type: "bit", nullable: false),
                    NextHigherLineManagerDecision = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuperviseeInitialsAndSurname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupervisorInitialsAndSurname = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalAssistancePlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MidTermKraEvaluations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MidTermReviewId = table.Column<int>(type: "int", nullable: false),
                    KraNumber = table.Column<int>(type: "int", nullable: false),
                    KraDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AchievementStandard = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupervisorComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnRating = table.Column<int>(type: "int", nullable: false),
                    SupervisorRating = table.Column<int>(type: "int", nullable: false),
                    AgreedRating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MidTermKraEvaluations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MidTermKraEvaluations_MidTermReviews_MidTermReviewId",
                        column: x => x.MidTermReviewId,
                        principalTable: "MidTermReviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MidTermKraEvaluations_MidTermReviewId",
                table: "MidTermKraEvaluations",
                column: "MidTermReviewId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MidTermKraEvaluations");

            migrationBuilder.DropTable(
                name: "PersonalAssistancePlans");

            migrationBuilder.DropTable(
                name: "MidTermReviews");

            migrationBuilder.AlterColumn<string>(
                name: "PersalNumber",
                table: "Employees",
                type: "nvarchar(9)",
                maxLength: 9,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "IdentificationNumber",
                table: "Employees",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AgeGroup",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AppointmentDateInCurrentRank",
                table: "Employees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AppointmentDateInDCS",
                table: "Employees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentRankPostLevel",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostDesignation",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RelatedOSDDescription",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupervisorRankPostLevel",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupervisorSurnameInitials",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMDSSystems.Migrations
{
    /// <inheritdoc />
    public partial class AddPDPTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SalaryLevel",
                table: "PerformanceAgreements",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "JobRelatedTasks",
                table: "PerformanceAgreements",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "JobPurpose",
                table: "PerformanceAgreements",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Weight",
                table: "KRAs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Standards",
                table: "KRAs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "KRAs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "GAFs",
                table: "KRAs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "BathoPele",
                table: "KRAs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Activities",
                table: "KRAs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "PDPModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersalNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Directorate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Branch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalaryLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Race = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Disabled = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisabilityDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgeGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupervisorPosition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EducationalBackground = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Workshops = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentStudies = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bursary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BursaryDuration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobRequirementsVsTrainingRequired = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Declaration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeDeclarationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeDeclarationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CapturedOnDatabase = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCaptured = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDPModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PDPJobRequirements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PDPModelId = table.Column<int>(type: "int", nullable: false),
                    Task = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Training = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LearningType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NQFLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cost = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Impact = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDPJobRequirements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PDPJobRequirements_PDPModels_PDPModelId",
                        column: x => x.PDPModelId,
                        principalTable: "PDPModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PDPQualifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PDPModelId = table.Column<int>(type: "int", nullable: false),
                    Qualification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NQF = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearCompleted = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDPQualifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PDPQualifications_PDPModels_PDPModelId",
                        column: x => x.PDPModelId,
                        principalTable: "PDPModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PDPJobRequirements_PDPModelId",
                table: "PDPJobRequirements",
                column: "PDPModelId");

            migrationBuilder.CreateIndex(
                name: "IX_PDPQualifications_PDPModelId",
                table: "PDPQualifications",
                column: "PDPModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PDPJobRequirements");

            migrationBuilder.DropTable(
                name: "PDPQualifications");

            migrationBuilder.DropTable(
                name: "PDPModels");

            migrationBuilder.AlterColumn<string>(
                name: "SalaryLevel",
                table: "PerformanceAgreements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "JobRelatedTasks",
                table: "PerformanceAgreements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "JobPurpose",
                table: "PerformanceAgreements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Weight",
                table: "KRAs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Standards",
                table: "KRAs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "KRAs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GAFs",
                table: "KRAs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BathoPele",
                table: "KRAs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Activities",
                table: "KRAs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}

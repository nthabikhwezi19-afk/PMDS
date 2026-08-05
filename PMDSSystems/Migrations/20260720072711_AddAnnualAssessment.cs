using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMDSSystems.Migrations
{
    /// <inheritdoc />
    public partial class AddAnnualAssessment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnnualAssessments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersalNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KRA1Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KRA2Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KRA3Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KRA4Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KRA1Weight = table.Column<int>(type: "int", nullable: false),
                    KRA2Weight = table.Column<int>(type: "int", nullable: false),
                    KRA3Weight = table.Column<int>(type: "int", nullable: false),
                    KRA4Weight = table.Column<int>(type: "int", nullable: false),
                    KRA1Achievement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KRA2Achievement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KRA3Achievement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KRA4Achievement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KRA1_OR = table.Column<int>(type: "int", nullable: false),
                    KRA1_SR = table.Column<int>(type: "int", nullable: false),
                    KRA1_AR = table.Column<int>(type: "int", nullable: false),
                    KRA2_OR = table.Column<int>(type: "int", nullable: false),
                    KRA2_SR = table.Column<int>(type: "int", nullable: false),
                    KRA2_AR = table.Column<int>(type: "int", nullable: false),
                    KRA3_OR = table.Column<int>(type: "int", nullable: false),
                    KRA3_SR = table.Column<int>(type: "int", nullable: false),
                    KRA3_AR = table.Column<int>(type: "int", nullable: false),
                    KRA4_OR = table.Column<int>(type: "int", nullable: false),
                    KRA4_SR = table.Column<int>(type: "int", nullable: false),
                    KRA4_AR = table.Column<int>(type: "int", nullable: false),
                    HasDispute = table.Column<bool>(type: "bit", nullable: false),
                    DisputeKRA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupervisorSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateSigned = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnualAssessments", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnualAssessments");
        }
    }
}

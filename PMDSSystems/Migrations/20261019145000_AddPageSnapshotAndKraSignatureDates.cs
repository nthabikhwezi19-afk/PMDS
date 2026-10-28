using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMDSSystems.Migrations
{
    public partial class AddPageSnapshotAndKraSignatureDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PageSnapshotJson",
                table: "PerformanceAgreements",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PageSnapshotJson",
                table: "MidTermReviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PageSnapshotJson",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmployeeSignatureDate",
                table: "KRAs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SupervisorSignatureDate",
                table: "KRAs",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PageSnapshotJson",
                table: "PerformanceAgreements");

            migrationBuilder.DropColumn(
                name: "PageSnapshotJson",
                table: "MidTermReviews");

            migrationBuilder.DropColumn(
                name: "PageSnapshotJson",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "EmployeeSignatureDate",
                table: "KRAs");

            migrationBuilder.DropColumn(
                name: "SupervisorSignatureDate",
                table: "KRAs");
        }
    }
}

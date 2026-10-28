using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMDSSystems.Migrations
{
    /// <inheritdoc />
    public partial class AddPerformanceAgreementSubmissionStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "KRAs");

            migrationBuilder.DropColumn(
                name: "SubmittedToSupervisor",
                table: "KRAs");

            migrationBuilder.DropColumn(
                name: "SubmittedToSupervisorDate",
                table: "KRAs");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "PerformanceAgreements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "SubmittedToSupervisor",
                table: "PerformanceAgreements",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedToSupervisorDate",
                table: "PerformanceAgreements",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "PerformanceAgreements");

            migrationBuilder.DropColumn(
                name: "SubmittedToSupervisor",
                table: "PerformanceAgreements");

            migrationBuilder.DropColumn(
                name: "SubmittedToSupervisorDate",
                table: "PerformanceAgreements");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "KRAs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "SubmittedToSupervisor",
                table: "KRAs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedToSupervisorDate",
                table: "KRAs",
                type: "datetime2",
                nullable: true);
        }
    }
}

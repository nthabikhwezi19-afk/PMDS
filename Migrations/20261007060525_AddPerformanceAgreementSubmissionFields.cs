using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMDSSystems.Migrations
{
    /// <inheritdoc />
    public partial class AddPerformanceAgreementSubmissionFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EndYear",
                table: "PDPs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StartYear",
                table: "PDPs",
                type: "int",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndYear",
                table: "PDPs");

            migrationBuilder.DropColumn(
                name: "StartYear",
                table: "PDPs");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "KRAs");

            migrationBuilder.DropColumn(
                name: "SubmittedToSupervisor",
                table: "KRAs");

            migrationBuilder.DropColumn(
                name: "SubmittedToSupervisorDate",
                table: "KRAs");
        }
    }
}

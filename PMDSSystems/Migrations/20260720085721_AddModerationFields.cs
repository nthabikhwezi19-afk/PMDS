using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMDSSystems.Migrations
{
    /// <inheritdoc />
    public partial class AddModerationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChairpersonName",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ChairpersonSignature",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CommitteeMembers",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FinalModerationCategory",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FinalModerationPercentage",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ModerationCategory",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModerationDate",
                table: "AnnualAssessments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModerationPercentage",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChairpersonName",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "ChairpersonSignature",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "CommitteeMembers",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "FinalModerationCategory",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "FinalModerationPercentage",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "ModerationCategory",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "ModerationDate",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "ModerationPercentage",
                table: "AnnualAssessments");
        }
    }
}

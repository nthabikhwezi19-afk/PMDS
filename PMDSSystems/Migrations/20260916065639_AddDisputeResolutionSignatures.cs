using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMDSSystems.Migrations
{
    /// <inheritdoc />
    public partial class AddDisputeResolutionSignatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HigherManagerName",
                table: "MidTermReviews",
                newName: "HigherManagerSurnameInitials");

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
                name: "Email",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ModerationPercentage",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ModerationCategory",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FinalModerationPercentage",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FinalModerationCategory",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CommitteeMembers",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ChairpersonSignature",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ChairpersonName",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ChairpersonInitials",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChairpersonSurname",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommitteeMember1Initials",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommitteeMember1Signature",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommitteeMember1Surname",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommitteeMember2Initials",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommitteeMember2Signature",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommitteeMember2Surname",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommitteeMember3Initials",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommitteeMember3Signature",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommitteeMember3Surname",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommitteeMember4Initials",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommitteeMember4Signature",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommitteeMember4Surname",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommitteeMember5Initials",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommitteeMember5Signature",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommitteeMember5Surname",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "HigherLineManagerDate",
                table: "AnnualAssessments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HigherLineManagerDecision",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HigherLineManagerSignature",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HigherLineManagerSurnameInitials",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChairpersonInitials",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "ChairpersonSurname",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "CommitteeMember1Initials",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "CommitteeMember1Signature",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "CommitteeMember1Surname",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "CommitteeMember2Initials",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "CommitteeMember2Signature",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "CommitteeMember2Surname",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "CommitteeMember3Initials",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "CommitteeMember3Signature",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "CommitteeMember3Surname",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "CommitteeMember4Initials",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "CommitteeMember4Signature",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "CommitteeMember4Surname",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "CommitteeMember5Initials",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "CommitteeMember5Signature",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "CommitteeMember5Surname",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "HigherLineManagerDate",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "HigherLineManagerDecision",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "HigherLineManagerSignature",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "HigherLineManagerSurnameInitials",
                table: "AnnualAssessments");

            migrationBuilder.RenameColumn(
                name: "HigherManagerSurnameInitials",
                table: "MidTermReviews",
                newName: "HigherManagerName");

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
                name: "Email",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ModerationPercentage",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ModerationCategory",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FinalModerationPercentage",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FinalModerationCategory",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CommitteeMembers",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ChairpersonSignature",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ChairpersonName",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}

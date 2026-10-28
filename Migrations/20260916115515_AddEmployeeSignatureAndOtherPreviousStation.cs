using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMDSSystems.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeSignatureAndOtherPreviousStation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SuperviseeSignature",
                table: "PersonalAssistancePlans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SuperviseeSignatureDate",
                table: "PersonalAssistancePlans",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SuperviseeSurnameInitials",
                table: "PersonalAssistancePlans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupervisorSignature",
                table: "PersonalAssistancePlans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SupervisorSignatureDate",
                table: "PersonalAssistancePlans",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupervisorSurnameInitials",
                table: "PersonalAssistancePlans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeSignature",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherPreviousStation",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SuperviseeSignature",
                table: "PersonalAssistancePlans");

            migrationBuilder.DropColumn(
                name: "SuperviseeSignatureDate",
                table: "PersonalAssistancePlans");

            migrationBuilder.DropColumn(
                name: "SuperviseeSurnameInitials",
                table: "PersonalAssistancePlans");

            migrationBuilder.DropColumn(
                name: "SupervisorSignature",
                table: "PersonalAssistancePlans");

            migrationBuilder.DropColumn(
                name: "SupervisorSignatureDate",
                table: "PersonalAssistancePlans");

            migrationBuilder.DropColumn(
                name: "SupervisorSurnameInitials",
                table: "PersonalAssistancePlans");

            migrationBuilder.DropColumn(
                name: "EmployeeSignature",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "OtherPreviousStation",
                table: "Employees");
        }
    }
}

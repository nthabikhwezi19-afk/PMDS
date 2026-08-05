using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMDSSystems.Migrations
{
    /// <inheritdoc />
    public partial class addworkinfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMDSSystems.Migrations
{
    /// <inheritdoc />
    public partial class Dates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentRankDate",
                table: "Employees");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AppointmentDate",
                table: "PMDSForms",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "AppointmentDateInCurrentRank",
                table: "PMDSForms",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentDateInCurrentRank",
                table: "PMDSForms");

            migrationBuilder.AlterColumn<string>(
                name: "AppointmentDate",
                table: "PMDSForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CurrentRankDate",
                table: "Employees",
                type: "datetime2",
                nullable: true);
        }
    }
}

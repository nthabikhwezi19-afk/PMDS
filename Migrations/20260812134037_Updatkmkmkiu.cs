using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMDSSystems.Migrations
{
    /// <inheritdoc />
    public partial class Updatkmkmkiu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OSDDescription",
                table: "PMDSForms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreviousCyclePerformance",
                table: "PMDSForms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreviousStation",
                table: "PMDSForms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TransferDate",
                table: "PMDSForms",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OSDDescription",
                table: "PMDSForms");

            migrationBuilder.DropColumn(
                name: "PreviousCyclePerformance",
                table: "PMDSForms");

            migrationBuilder.DropColumn(
                name: "PreviousStation",
                table: "PMDSForms");

            migrationBuilder.DropColumn(
                name: "TransferDate",
                table: "PMDSForms");
        }
    }
}

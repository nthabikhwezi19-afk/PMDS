using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMDSSystems.Migrations
{
    /// <inheritdoc />
    public partial class NewModelss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentDateInDcs",
                table: "PMDSForms");

            migrationBuilder.DropColumn(
                name: "SupervisorRank",
                table: "PMDSForms");

            migrationBuilder.AddColumn<string>(
                name: "SupervisorSurnameInitials",
                table: "PMDSForms",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupervisorSurnameInitials",
                table: "PMDSForms");

            migrationBuilder.AddColumn<DateTime>(
                name: "AppointmentDateInDcs",
                table: "PMDSForms",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SupervisorRank",
                table: "PMDSForms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

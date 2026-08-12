using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMDSSystems.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedEmployeeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmployeeSignature",
                table: "Employees",
                newName: "PreviousStation");

            migrationBuilder.AddColumn<int>( 
                name: "PreviousCyclePerformance",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SignatureDate",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "TransferDate",
                table: "Employees",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviousCyclePerformance",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "SignatureDate",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "TransferDate",
                table: "Employees");

            migrationBuilder.RenameColumn(
                name: "PreviousStation",
                table: "Employees",
                newName: "EmployeeSignature");
        }
    }
}

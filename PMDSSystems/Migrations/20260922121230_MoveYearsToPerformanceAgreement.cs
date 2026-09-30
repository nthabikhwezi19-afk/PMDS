using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMDSSystems.Migrations
{
    /// <inheritdoc />
    public partial class MoveYearsToPerformanceAgreement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EndYear",
                table: "PerformanceAgreements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StartYear",
                table: "PerformanceAgreements",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndYear",
                table: "PerformanceAgreements");

            migrationBuilder.DropColumn(
                name: "StartYear",
                table: "PerformanceAgreements");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMDSSystems.Migrations
{
    /// <inheritdoc />
    public partial class AddKraNamesToAnnualAssessment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KRA1Name",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KRA2Name",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KRA3Name",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KRA4Name",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KRA1Name",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "KRA2Name",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "KRA3Name",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "KRA4Name",
                table: "AnnualAssessments");
        }
    }
}

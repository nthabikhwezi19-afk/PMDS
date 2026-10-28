using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMDSSystems.Migrations
{
    /// <inheritdoc />
    public partial class AddKra5AndKra6ToAnnualAssessment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KRA5Achievement",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KRA5Comment",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KRA5Name",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KRA5Weight",
                table: "AnnualAssessments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "KRA5_AR",
                table: "AnnualAssessments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "KRA5_OR",
                table: "AnnualAssessments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "KRA5_SR",
                table: "AnnualAssessments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "KRA6Achievement",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KRA6Comment",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KRA6Name",
                table: "AnnualAssessments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KRA6Weight",
                table: "AnnualAssessments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "KRA6_AR",
                table: "AnnualAssessments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "KRA6_OR",
                table: "AnnualAssessments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "KRA6_SR",
                table: "AnnualAssessments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KRA5Achievement",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "KRA5Comment",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "KRA5Name",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "KRA5Weight",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "KRA5_AR",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "KRA5_OR",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "KRA5_SR",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "KRA6Achievement",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "KRA6Comment",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "KRA6Name",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "KRA6Weight",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "KRA6_AR",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "KRA6_OR",
                table: "AnnualAssessments");

            migrationBuilder.DropColumn(
                name: "KRA6_SR",
                table: "AnnualAssessments");
        }
    }
}

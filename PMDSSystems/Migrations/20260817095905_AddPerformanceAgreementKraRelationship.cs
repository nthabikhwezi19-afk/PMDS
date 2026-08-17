using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMDSSystems.Migrations
{
    /// <inheritdoc />
    public partial class AddPerformanceAgreementKraRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KRAs_PerformanceAgreements_PerformanceAgreementId",
                table: "KRAs");

            migrationBuilder.AlterColumn<int>(
                name: "PerformanceAgreementId",
                table: "KRAs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PreviousCyclePerformance",
                table: "Employees",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_KRAs_PerformanceAgreements_PerformanceAgreementId",
                table: "KRAs",
                column: "PerformanceAgreementId",
                principalTable: "PerformanceAgreements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KRAs_PerformanceAgreements_PerformanceAgreementId",
                table: "KRAs");

            migrationBuilder.AlterColumn<int>(
                name: "PerformanceAgreementId",
                table: "KRAs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PreviousCyclePerformance",
                table: "Employees",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_KRAs_PerformanceAgreements_PerformanceAgreementId",
                table: "KRAs",
                column: "PerformanceAgreementId",
                principalTable: "PerformanceAgreements",
                principalColumn: "Id");
        }
    }
}

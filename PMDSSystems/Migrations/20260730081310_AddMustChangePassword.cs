using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMDSSystems.Migrations
{
    /// <inheritdoc />
    public partial class AddMustChangePassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "PMDSForms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "MustChangePassword",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_PMDSForms_EmployeeId",
                table: "PMDSForms",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PMDSForms_Employees_EmployeeId",
                table: "PMDSForms",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PMDSForms_Employees_EmployeeId",
                table: "PMDSForms");

            migrationBuilder.DropIndex(
                name: "IX_PMDSForms_EmployeeId",
                table: "PMDSForms");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "PMDSForms");

            migrationBuilder.DropColumn(
                name: "MustChangePassword",
                table: "Employees");
        }
    }
}

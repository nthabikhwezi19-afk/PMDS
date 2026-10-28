using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMDSSystems.Migrations
{
    /// <inheritdoc />
    public partial class AddPDPDetailsAndTraining : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PDPJobRequirements_PDPModels_PDPModelId",
                table: "PDPJobRequirements");

            migrationBuilder.DropTable(
                name: "PDPQualifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PDPModels",
                table: "PDPModels");

            migrationBuilder.RenameTable(
                name: "PDPModels",
                newName: "PDPs");

            migrationBuilder.RenameColumn(
                name: "JobRequirementsVsTrainingRequired",
                table: "PDPs",
                newName: "SupervisorName");

            migrationBuilder.RenameColumn(
                name: "EmployeeDeclarationName",
                table: "PDPs",
                newName: "Supervisor");

            migrationBuilder.RenameColumn(
                name: "EmployeeDeclarationDate",
                table: "PDPs",
                newName: "EmployeeDate");

            migrationBuilder.RenameColumn(
                name: "EducationalBackground",
                table: "PDPs",
                newName: "Goal");

            migrationBuilder.RenameColumn(
                name: "Declaration",
                table: "PDPs",
                newName: "EmployeeSignature");

            migrationBuilder.AddColumn<string>(
                name: "ActionPlan",
                table: "PDPs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PDPs",
                table: "PDPs",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PDPEducations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PDPModelId = table.Column<int>(type: "int", nullable: false),
                    Qualification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NQF = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Year = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDPEducations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PDPEducations_PDPs_PDPModelId",
                        column: x => x.PDPModelId,
                        principalTable: "PDPs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PDPEducations_PDPModelId",
                table: "PDPEducations",
                column: "PDPModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_PDPJobRequirements_PDPs_PDPModelId",
                table: "PDPJobRequirements",
                column: "PDPModelId",
                principalTable: "PDPs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PDPJobRequirements_PDPs_PDPModelId",
                table: "PDPJobRequirements");

            migrationBuilder.DropTable(
                name: "PDPEducations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PDPs",
                table: "PDPs");

            migrationBuilder.DropColumn(
                name: "ActionPlan",
                table: "PDPs");

            migrationBuilder.RenameTable(
                name: "PDPs",
                newName: "PDPModels");

            migrationBuilder.RenameColumn(
                name: "SupervisorName",
                table: "PDPModels",
                newName: "JobRequirementsVsTrainingRequired");

            migrationBuilder.RenameColumn(
                name: "Supervisor",
                table: "PDPModels",
                newName: "EmployeeDeclarationName");

            migrationBuilder.RenameColumn(
                name: "Goal",
                table: "PDPModels",
                newName: "EducationalBackground");

            migrationBuilder.RenameColumn(
                name: "EmployeeSignature",
                table: "PDPModels",
                newName: "Declaration");

            migrationBuilder.RenameColumn(
                name: "EmployeeDate",
                table: "PDPModels",
                newName: "EmployeeDeclarationDate");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PDPModels",
                table: "PDPModels",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PDPQualifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PDPModelId = table.Column<int>(type: "int", nullable: false),
                    NQF = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Qualification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearCompleted = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PDPQualifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PDPQualifications_PDPModels_PDPModelId",
                        column: x => x.PDPModelId,
                        principalTable: "PDPModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PDPQualifications_PDPModelId",
                table: "PDPQualifications",
                column: "PDPModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_PDPJobRequirements_PDPModels_PDPModelId",
                table: "PDPJobRequirements",
                column: "PDPModelId",
                principalTable: "PDPModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMDSSystems.Migrations
{
    /// <inheritdoc />
    public partial class AddPerformanceAgreement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PerformanceAgreements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobPurpose = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobRelatedTasks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SalaryLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfKRAs = table.Column<int>(type: "int", nullable: false),
                    JobKnowledge = table.Column<bool>(type: "bit", nullable: false),
                    Responsibility = table.Column<bool>(type: "bit", nullable: false),
                    QualityOfWork = table.Column<bool>(type: "bit", nullable: false),
                    TechnicalSkills = table.Column<bool>(type: "bit", nullable: false),
                    Reliability = table.Column<bool>(type: "bit", nullable: false),
                    Communication = table.Column<bool>(type: "bit", nullable: false),
                    TeamWork = table.Column<bool>(type: "bit", nullable: false),
                    Leadership = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerformanceAgreements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KRAs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weight = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Activities = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Standards = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BathoPele = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GAFs = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PerformanceAgreementId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KRAs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KRAs_PerformanceAgreements_PerformanceAgreementId",
                        column: x => x.PerformanceAgreementId,
                        principalTable: "PerformanceAgreements",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_KRAs_PerformanceAgreementId",
                table: "KRAs",
                column: "PerformanceAgreementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KRAs");

            migrationBuilder.DropTable(
                name: "PerformanceAgreements");
        }
    }
}

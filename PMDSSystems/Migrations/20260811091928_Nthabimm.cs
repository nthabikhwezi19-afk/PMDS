using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMDSSystems.Migrations
{
    /// <inheritdoc />
    public partial class Nthabimm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnnualAssessments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersalNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KRA1Weight = table.Column<int>(type: "int", nullable: false),
                    KRA2Weight = table.Column<int>(type: "int", nullable: false),
                    KRA3Weight = table.Column<int>(type: "int", nullable: false),
                    KRA4Weight = table.Column<int>(type: "int", nullable: false),
                    KRA1Achievement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KRA2Achievement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KRA3Achievement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KRA4Achievement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KRA1Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KRA2Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KRA3Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KRA4Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KRA1_OR = table.Column<int>(type: "int", nullable: false),
                    KRA1_SR = table.Column<int>(type: "int", nullable: false),
                    KRA1_AR = table.Column<int>(type: "int", nullable: false),
                    KRA2_OR = table.Column<int>(type: "int", nullable: false),
                    KRA2_SR = table.Column<int>(type: "int", nullable: false),
                    KRA2_AR = table.Column<int>(type: "int", nullable: false),
                    KRA3_OR = table.Column<int>(type: "int", nullable: false),
                    KRA3_SR = table.Column<int>(type: "int", nullable: false),
                    KRA3_AR = table.Column<int>(type: "int", nullable: false),
                    KRA4_OR = table.Column<int>(type: "int", nullable: false),
                    KRA4_SR = table.Column<int>(type: "int", nullable: false),
                    KRA4_AR = table.Column<int>(type: "int", nullable: false),
                    HasDispute = table.Column<bool>(type: "bit", nullable: false),
                    DisputeKRA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupervisorSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateSigned = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModerationCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModerationPercentage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChairpersonSignature = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FinalModerationPercentage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FinalModerationCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChairpersonName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModerationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CommitteeMembers = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnualAssessments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersalNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Initials = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentificationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupervisorId = table.Column<int>(type: "int", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SalaryLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostDesignation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppointmentDateInCurrentRank = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AppointmentInDcsDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CurrentRankDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SupervisorSurnameInitials = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupervisorRankPostLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OSDDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranchOrRegion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MustChangePassword = table.Column<bool>(type: "bit", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgeGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Race = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasDisability = table.Column<bool>(type: "bit", nullable: true),
                    NatureOfDisability = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeclarationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Employees_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MidTermReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReviewPeriod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasDispute = table.Column<bool>(type: "bit", nullable: false),
                    DisputedKraNumbers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HigherManagerDecision = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuperviseeSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuperviseeSignDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SupervisorSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupervisorSignDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HigherManagerSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HigherManagerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HigherManagerSignDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MidTermReviews", x => x.Id);
                });

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
                name: "PerformanceCycles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerformanceCycles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonalAssistancePlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Post = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PersalNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupervisorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RemedialSteps = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Month1Progress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Month2Progress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Month3Progress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Month4Progress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Month5Progress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Month6Progress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPerformanceUpToStandard = table.Column<bool>(type: "bit", nullable: false),
                    NextHigherLineManagerDecision = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuperviseeInitialsAndSurname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupervisorInitialsAndSurname = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalAssistancePlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PMDSForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    PersalNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SurnameInitials = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Directorate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostDesignation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupervisorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppointmentDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentRank = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RelatedOSDDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupervisorSurnameInitials = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupervisorRankPostLevel = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PMDSForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PMDSForms_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MidTermKraEvaluations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MidTermReviewId = table.Column<int>(type: "int", nullable: false),
                    KraNumber = table.Column<int>(type: "int", nullable: false),
                    KraDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AchievementStandard = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupervisorComments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OwnRating = table.Column<int>(type: "int", nullable: false),
                    SupervisorRating = table.Column<int>(type: "int", nullable: false),
                    AgreedRating = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MidTermKraEvaluations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MidTermKraEvaluations_MidTermReviews_MidTermReviewId",
                        column: x => x.MidTermReviewId,
                        principalTable: "MidTermReviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "PerformanceReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    CycleId = table.Column<int>(type: "int", nullable: false),
                    KPI1 = table.Column<int>(type: "int", nullable: false),
                    KPI2 = table.Column<int>(type: "int", nullable: false),
                    KPI3 = table.Column<int>(type: "int", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FinalScore = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerformanceReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerformanceReviews_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PerformanceReviews_PerformanceCycles_CycleId",
                        column: x => x.CycleId,
                        principalTable: "PerformanceCycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SupervisorId",
                table: "Employees",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_KRAs_PerformanceAgreementId",
                table: "KRAs",
                column: "PerformanceAgreementId");

            migrationBuilder.CreateIndex(
                name: "IX_MidTermKraEvaluations_MidTermReviewId",
                table: "MidTermKraEvaluations",
                column: "MidTermReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReviews_CycleId",
                table: "PerformanceReviews",
                column: "CycleId");

            migrationBuilder.CreateIndex(
                name: "IX_PerformanceReviews_EmployeeId",
                table: "PerformanceReviews",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PMDSForms_EmployeeId",
                table: "PMDSForms",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnualAssessments");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "KRAs");

            migrationBuilder.DropTable(
                name: "MidTermKraEvaluations");

            migrationBuilder.DropTable(
                name: "PerformanceReviews");

            migrationBuilder.DropTable(
                name: "PersonalAssistancePlans");

            migrationBuilder.DropTable(
                name: "PMDSForms");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "PerformanceAgreements");

            migrationBuilder.DropTable(
                name: "MidTermReviews");

            migrationBuilder.DropTable(
                name: "PerformanceCycles");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}

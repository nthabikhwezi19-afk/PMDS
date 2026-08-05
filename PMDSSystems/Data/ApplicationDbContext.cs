using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PMDSSystems.Models;
using YourProjectName.Models;

namespace PMDSSystems.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<PerformanceCycle> PerformanceCycles { get; set; }

        public DbSet<PerformanceReview> PerformanceReviews { get; set; }

        public DbSet<PerformanceAgreement> PerformanceAgreements { get; set; }

        public DbSet<KRA> KRAs { get; set; }

        public DbSet<PMDSForm> PMDSForms { get; set; }

        public DbSet<MidTermReview> MidTermReviews { get; set; }

        public DbSet<MidTermKraEvaluation> MidTermKraEvaluations { get; set; }

        public DbSet<PersonalAssistancePlan> PersonalAssistancePlans { get; set; }

        public DbSet<AnnualAssessment> AnnualAssessments { get; set; }
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PMDSSystems.Models;
using YourProjectName.Models;

namespace PMDSSystems.Data
{
    // CHANGE THIS LINE - specify the user type
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>  // Add <IdentityUser>
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Your entity configurations here
        }
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PMDSSystems.Models;

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
        public DbSet<PDPModel> PDPs { get; set; }

        public DbSet<PDPEducation> PDPEducations { get; set; }

        public DbSet<PDPJobRequirement> PDPJobRequirements { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<KRA>()
                .HasOne(k => k.PerformanceAgreement)
                .WithMany(p => p.KRAs)
                .HasForeignKey(k => k.PerformanceAgreementId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PDPEducation>()
    .HasOne(e => e.PDPModel)
    .WithMany(p => p.Education)
    .HasForeignKey(e => e.PDPModelId)
    .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PDPJobRequirement>()
                .HasOne(j => j.PDPModel)
                .WithMany(p => p.JobRequirements)
                .HasForeignKey(j => j.PDPModelId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
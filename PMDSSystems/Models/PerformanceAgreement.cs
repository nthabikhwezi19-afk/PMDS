using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMDSSystems.Models
{
    public class PerformanceAgreement
    {
        public int Id { get; set; }

        public string? JobPurpose { get; set; }

        [Display(Name = "Job Related Tasks")]
        public string? JobRelatedTasks { get; set; }

        [Display(Name = "Salary Level")]
        public string? SalaryLevel { get; set; }

        [Display(Name = "Number of KRAs")]
        public int NumberOfKRAs { get; set; }

        public bool JobKnowledge { get; set; }
        public bool Responsibility { get; set; }
        public bool QualityOfWork { get; set; }
        public bool TechnicalSkills { get; set; }
        public bool Reliability { get; set; }
        public bool Communication { get; set; }
        public bool TeamWork { get; set; }
        public bool Leadership { get; set; }

        public List<KRA> KRAs { get; set; } = new List<KRA>();
    }


    public class KRA
    {
        public int Id { get; set; }

        public int PerformanceAgreementId { get; set; }

        [ForeignKey(nameof(PerformanceAgreementId))]
        public PerformanceAgreement? PerformanceAgreement { get; set; }

        public string? Name { get; set; }

        public string? Weight { get; set; }

        public string? Activities { get; set; }

        public string? Standards { get; set; }

        public string? BathoPele { get; set; }

        public string? GAFs { get; set; }
    }
}
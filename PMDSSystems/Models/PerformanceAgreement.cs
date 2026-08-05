using System.Collections.Generic;

namespace PMDSSystems.Models
{
    public class PerformanceAgreement
    {
        public int Id { get; set; }

        public string JobPurpose { get; set; }
        public string JobRelatedTasks { get; set; }

        public string SalaryLevel { get; set; }
        public int NumberOfKRAs { get; set; }

        public bool JobKnowledge { get; set; }
        public bool Responsibility { get; set; }
        public bool QualityOfWork { get; set; }
        public bool TechnicalSkills { get; set; }
        public bool Reliability { get; set; }
        public bool Communication { get; set; }
        public bool TeamWork { get; set; }
        public bool Leadership { get; set; }

        public List<KRA> KRAs { get; set; }
    }

    public class KRA
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Weight { get; set; }
        public string Activities { get; set; }
        public string Standards { get; set; }
        public string BathoPele { get; set; }
        public string GAFs { get; set; }
    }
}
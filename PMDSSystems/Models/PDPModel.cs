namespace PMDSSystems.Models
{
    public class PDPModel
    {
        public int Id { get; set; }

        // ==========================================
        // EMPLOYEE INFORMATION
        // ==========================================

        public string? Surname { get; set; }
        public string? PersalNo { get; set; }
        public string? Directorate { get; set; }
        public string? IdNumber { get; set; }
        public string? Branch { get; set; }
        public string? SalaryLevel { get; set; }
        public string? AgeGroup { get; set; }
        public string? Gender { get; set; }
        public string? Race { get; set; }
        public string? Disabled { get; set; }
        public string? DisabilityDetails { get; set; }

        // ==========================================
        // SUPERVISOR
        // ==========================================

        public string? SupervisorPosition { get; set; }
        public string? Supervisor { get; set; }

        // ==========================================
        // EDUCATIONAL / DEVELOPMENT INFORMATION
        // ==========================================

        public string? Workshops { get; set; }

        public string? CurrentStudies { get; set; }

        public string? Bursary { get; set; }

        public string? BursaryDuration { get; set; }

        // ==========================================
        // DECLARATION
        // ==========================================

        public string? EmployeeSignature { get; set; }

        public DateTime? EmployeeDate { get; set; }

        public string? SupervisorName { get; set; }

        // ==========================================
        // OFFICE USE ONLY
        // ==========================================

        public string? CapturedOnDatabase { get; set; }

        public DateTime? DateCaptured { get; set; }

        // ==========================================
        // PDP INFORMATION
        // ==========================================

        public string? Goal { get; set; }

        public string? ActionPlan { get; set; }

        // ==========================================
        // RELATIONSHIPS
        // ==========================================

        public List<PDPEducation> Education { get; set; }
            = new List<PDPEducation>();

        public List<PDPJobRequirement> JobRequirements { get; set; }
            = new List<PDPJobRequirement>();
    }
}
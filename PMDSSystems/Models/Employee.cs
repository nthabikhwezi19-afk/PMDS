using System.ComponentModel.DataAnnotations;

namespace PMDSSystems.Models
{
    public class Employee
    {
        public int Id { get; set; }

        // =========================
        // PERSONAL INFORMATION
        // =========================

      
        public string PersalNumber { get; set; } = string.Empty;

        
        public string FirstName { get; set; } = string.Empty;

      
        public string LastName { get; set; } = string.Empty;

        public string? Initials { get; set; }

        public string? Email { get; set; }

        public string? IdentificationNumber { get; set; }
        public int? SupervisorId { get; set; }
        public Employee? Supervisor { get; set; }
      //  public string SupervisorSurnameInitials { get; set; } = string.Empty;


        // =========================
        // WORK INFORMATION
        // =========================

        public string? Department { get; set; }

       
        public string? Position { get; set; } = string.Empty;

        public string? SalaryLevel { get; set; }

        public string? PostDesignation { get; set; }

        public string? PostLevel { get; set; }

        public DateTime? AppointmentDateInCurrentRank { get; set; }

        public DateTime? AppointmentInDcsDate { get; set; }

        public DateTime? CurrentRankDate { get; set; }

        public string? SupervisorSurnameInitials { get; set; }

        public string? SupervisorRankPostLevel { get; set; }

        public string? OSDDescription { get; set; }

        public string? BranchOrRegion { get; set; }


        // =========================
        // SUPERVISOR RELATIONSHIP
        // =========================

       


        // 🔗 Link to login user (Identity)
        public string? UserId { get; set; }

        // 🔗 Supervisor relationship
  
        // =========================
        // IDENTITY USER LINK
        // =========================



        public bool MustChangePassword { get; set; }

        // =========================
        // DEMOGRAPHICS
        // =========================

        public string? Gender { get; set; }

        public string? AgeGroup { get; set; }

        public string? Race { get; set; }

        public bool? HasDisability { get; set; }

        public string? NatureOfDisability { get; set; }


        // =========================
        // DECLARATION
        // =========================

        //public string EmployeeSignature { get; set; }
        public string? SignatureDate { get; set; }
        public String? PreviousStation { get; set; }
        public DateTime? TransferDate { get; set; }
        public int? PreviousCyclePerformance { get; set; }
        public DateTime? DeclarationDate { get; set; }
        
    }
}
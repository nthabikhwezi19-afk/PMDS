using System.ComponentModel.DataAnnotations;

namespace PMDSSystems.Models
{
    public class Employee
    {
        public int Id { get; set; }

        // =========================
        // PERSONAL INFORMATION
        // =========================

        [Required]
        public string PersalNumber { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        public string? Initials { get; set; }

        public string? Email { get; set; }

        public string? IdentificationNumber { get; set; }


        // =========================
        // WORK INFORMATION
        // =========================

        public string? Department { get; set; }

        [Required]
        public string Position { get; set; } = string.Empty;

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

        public int? SupervisorId { get; set; }

        public Employee? Supervisor { get; set; }


        // =========================
        // IDENTITY USER LINK
        // =========================

        public string? UserId { get; set; }
        public bool MustChangePassword { get; set; } = true;


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

        public string? EmployeeSignature { get; set; }

        public DateTime? DeclarationDate { get; set; }
    }
}
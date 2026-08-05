using System.ComponentModel.DataAnnotations;

namespace PMDSSystems.Models
{
    public class PMDSForm
    {
        public int Id { get; set; }

        // =========================
        // EMPLOYEE RELATIONSHIP
        // =========================

        [Required]
        public int EmployeeId { get; set; }

        public Employee? Employee { get; set; }


        // =========================
        // EMPLOYEE INFORMATION
        // =========================

        [Required]
        public string PersalNumber { get; set; } = string.Empty;

        public string SurnameInitials { get; set; } = string.Empty;

        public string Directorate { get; set; } = string.Empty;

        public string PostDesignation { get; set; } = string.Empty;

        public string SupervisorName { get; set; } = string.Empty;

        public string AppointmentDate { get; set; } = string.Empty;

        public string CurrentRank { get; set; } = string.Empty;

        public string RelatedOSDDescription { get; set; } = string.Empty;
    }
}
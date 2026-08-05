using System;
using System.ComponentModel.DataAnnotations;

namespace PMDSSystems.Models
{
    public class PersonalAssistancePlan
    {
        [Key]
        public int Id { get; set; }

        // --- A. Personal Particulars ---
        [Required]
        public string EmployeeName { get; set; } = string.Empty;

        [Required]
        public string Post { get; set; } = string.Empty;

        [Required]
        public string PersalNo { get; set; } = string.Empty; // Persal/ID Number

        [Required]
        public string SupervisorName { get; set; } = string.Empty;

        // --- B. Personal Assistance Plan ---
        // Storing remedial steps listed by the supervisor
        public string RemedialSteps { get; set; } = string.Empty;

        // --- C. Monthly Progress Report (Months 1 to 6) ---
        public string Month1Progress { get; set; } = string.Empty;
        public string Month2Progress { get; set; } = string.Empty;
        public string Month3Progress { get; set; } = string.Empty;
        public string Month4Progress { get; set; } = string.Empty;
        public string Month5Progress { get; set; } = string.Empty;
        public string Month6Progress { get; set; } = string.Empty;

        // --- Final Decision ---
        public bool IsPerformanceUpToStandard { get; set; } // Yes/No
        public string? NextHigherLineManagerDecision { get; set; } // If performance is unsatisfactory

        // --- Sign-offs ---
        public string? SuperviseeInitialsAndSurname { get; set; }
        public string? SupervisorInitialsAndSurname { get; set; }
    }
}
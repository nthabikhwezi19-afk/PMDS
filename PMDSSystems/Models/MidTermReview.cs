using PMDSSystems.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourProjectName.Models
{
    public class MidTermReview
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string EmployeeId { get; set; } = string.Empty; // Linked to your User/Employee table

        [Required]
        public string ReviewPeriod { get; set; } = "1 April 2024 - 31 October 2025";

        // Navigation property linking to the individual KRA rows (1-to-many relationship)
        public List<MidTermKraEvaluation> KraEvaluations { get; set; } = new();

        // --- Dispute Fields (Part C2) ---
        public bool HasDispute { get; set; }
        public string? DisputedKraNumbers { get; set; } // Stored as string, e.g., "KRA 1, KRA 3"
        public string? HigherManagerDecision { get; set; }

        // --- Signatures & Dates ---
        public string? SuperviseeSignature { get; set; }
        public DateTime? SuperviseeSignDate { get; set; }

        public string? SupervisorSignature { get; set; }
        public DateTime? SupervisorSignDate { get; set; }

        public string? HigherManagerSignature { get; set; }
        public string? HigherManagerName { get; set; }
        public DateTime? HigherManagerSignDate { get; set; }
    }
}

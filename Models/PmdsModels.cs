using System;
using System.ComponentModel.DataAnnotations;

namespace PMDS.Models
{
    public class AnnualAssessment
    {
        [Key]
        public int Id { get; set; }

        public string CyclePeriod { get; set; } = "1 April 2024 – 31 March 2025";

        // --- KRA 1 ---
        [Required(ErrorMessage = "KRA 1 Weight is required.")]
        [Range(0, 100, ErrorMessage = "Weight must be between 0 and 100.")]
        public int Kra1Weight { get; set; }

        [Required(ErrorMessage = "KRA 1 Achievement motivation is required.")]
        public string Kra1Achievement { get; set; }
        public string Kra1SupervisorComments { get; set; }

        // --- KRA 2 ---
        [Required(ErrorMessage = "KRA 2 Weight is required.")]
        [Range(0, 100, ErrorMessage = "Weight must be between 0 and 100.")]
        public int Kra2Weight { get; set; }

        [Required(ErrorMessage = "KRA 2 Achievement motivation is required.")]
        public string Kra2Achievement { get; set; }
        public string Kra2SupervisorComments { get; set; }

        // --- KRA 3 ---
        [Required(ErrorMessage = "KRA 3 Weight is required.")]
        [Range(0, 100, ErrorMessage = "Weight must be between 0 and 100.")]
        public int Kra3Weight { get; set; }

        [Required(ErrorMessage = "KRA 3 Achievement motivation is required.")]
        public string Kra3Achievement { get; set; }
        public string Kra3SupervisorComments { get; set; }

        // --- KRA 4 ---
        [Required(ErrorMessage = "KRA 4 Weight is required.")]
        [Range(0, 100, ErrorMessage = "Weight must be between 0 and 100.")]
        public int Kra4Weight { get; set; }

        [Required(ErrorMessage = "KRA 4 Achievement motivation is required.")]
        public string Kra4Achievement { get; set; }
        public string Kra4SupervisorComments { get; set; }

        // --- Ratings (Part D2) ---
        [Required(ErrorMessage = "Self rating (OR) is required for KRA 1.")]
        public int? Kra1OwnRating { get; set; }

        [Required(ErrorMessage = "Supervisor rating (SR) is required for KRA 1.")]
        public int? Kra1SupervisorRating { get; set; }

        [Required(ErrorMessage = "Agreed rating (AR) is required for KRA 1.")]
        public int? Kra1AgreedRating { get; set; }

        [Required(ErrorMessage = "Self rating (OR) is required for KRA 2.")]
        public int? Kra2OwnRating { get; set; }

        [Required(ErrorMessage = "Supervisor rating (SR) is required for KRA 2.")]
        public int? Kra2SupervisorRating { get; set; }

        [Required(ErrorMessage = "Agreed rating (AR) is required for KRA 2.")]
        public int? Kra2AgreedRating { get; set; }

        [Required(ErrorMessage = "Self rating (OR) is required for KRA 3.")]
        public int? Kra3OwnRating { get; set; }

        [Required(ErrorMessage = "Supervisor rating (SR) is required for KRA 3.")]
        public int? Kra3SupervisorRating { get; set; }

        [Required(ErrorMessage = "Agreed rating (AR) is required for KRA 3.")]
        public int? Kra3AgreedRating { get; set; }

        [Required(ErrorMessage = "Self rating (OR) is required for KRA 4.")]
        public int? Kra4OwnRating { get; set; }

        [Required(ErrorMessage = "Supervisor rating (SR) is required for KRA 4.")]
        public int? Kra4SupervisorRating { get; set; }

        [Required(ErrorMessage = "Agreed rating (AR) is required for KRA 4.")]
        public int? Kra4AgreedRating { get; set; }

        // Calculated Aggregate
        public decimal TotalWeightedScore { get; set; }

        // --- Disputes ---
        public bool IsDisputePresent { get; set; } = false;
        public string DisputedKraNumbers { get; set; }
        public string NextHigherLineManagerDecision { get; set; }

        // --- Clearances ---
        [Required(ErrorMessage = "Supervisee signature confirmation is required.")]
        public string SuperviseeSignatureText { get; set; }

        [Required(ErrorMessage = "Supervisee sign-off date is required.")]
        [DataType(DataType.Date)]
        public DateTime? SuperviseeSignatureDate { get; set; }

        [Required(ErrorMessage = "Supervisor signature confirmation is required.")]
        public string SupervisorSignatureText { get; set; }

        [Required(ErrorMessage = "Supervisor sign-off date is required.")]
        [DataType(DataType.Date)]
        public DateTime? SupervisorSignatureDate { get; set; }

        // Mediation Sign-Off (Conditional on Dispute)
        public string LineManagerSignatureText { get; set; }

        [DataType(DataType.Date)]
        public DateTime? LineManagerSignatureDate { get; set; }
    }

    public class PersonalAssistancePlan
    {
        [Key]
        public int Id { get; set; }

        // --- Step 1: Particulars ---
        [Required(ErrorMessage = "Employee name is required.")]
        public string EmployeeName { get; set; }

        [Required(ErrorMessage = "Post designation is required.")]
        public string Post { get; set; }

        [Required(ErrorMessage = "Persal Number is required.")]
        [RegularExpression(@"^[0-9]{8,9}$", ErrorMessage = "Persal number must be 8 or 9 digits.")]
        public string PersalNo { get; set; }

        [Required(ErrorMessage = "Supervisor name is required.")]
        public string SupervisorName { get; set; }

        // --- Step 2: Remedial ---
        [Required(ErrorMessage = "Please outline development actions.")]
        public string RemedialSteps { get; set; }

        // --- Step 3: Monthly Progress Reviews ---
        public string Month1Progress { get; set; }
        public string Month2Progress { get; set; }
        public string Month3Progress { get; set; }
        public string Month4Progress { get; set; }
        public string Month5Progress { get; set; }
        public string Month6Progress { get; set; }

        // --- Step 4: Decisions ---
        [Required(ErrorMessage = "Please determine if performance is up to standard.")]
        public bool IsPerformanceUpToStandard { get; set; } = true;
        public string NextHigherLineManagerDecision { get; set; }

        // --- Step 5: Confirmations ---
        [Required(ErrorMessage = "Supervisee confirmation initials & surname are required.")]
        public string SuperviseeInitialsAndSurname { get; set; }

        [Required(ErrorMessage = "Supervisor confirmation initials & surname are required.")]
        public string SupervisorInitialsAndSurname { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
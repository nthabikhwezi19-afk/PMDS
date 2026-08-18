using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMDSSystems.Models
{
    public class MidTermKraEvaluation
    {
        [Key]
        public int Id { get; set; }

        // ============================================================
        // FOREIGN KEY TO MID-TERM REVIEW
        // ============================================================

        public int MidTermReviewId { get; set; }

        [ForeignKey("MidTermReviewId")]
        public MidTermReview? MidTermReview { get; set; }

        // ============================================================
        // KRA INFORMATION
        // ============================================================

        [Required]
        public int KraNumber { get; set; }

        [Required]
        public string KraDescription { get; set; } = string.Empty;

        [Required]
        [Range(1, 100, ErrorMessage = "Weight must be between 1 and 100")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Weight { get; set; }

        // ============================================================
        // PART C1 - MOTIVATION
        // ============================================================

        public string AchievementStandard { get; set; } = string.Empty;

        public string SupervisorComments { get; set; } = string.Empty;

        // ============================================================
        // PART C2 - RATINGS
        // ============================================================

        [Range(1, 4, ErrorMessage = "Rating must be between 1 and 4")]
        public int OwnRating { get; set; }

        [Range(1, 4, ErrorMessage = "Rating must be between 1 and 4")]
        public int SupervisorRating { get; set; }

        [Range(1, 4, ErrorMessage = "Rating must be between 1 and 4")]
        public int AgreedRating { get; set; }
    }
}
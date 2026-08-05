using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourProjectName.Models
{
    public class MidTermKraEvaluation
    {
        [Key]
        public int Id { get; set; }

        // Foreign Key linking back to the parent MidTermReview
        public int MidTermReviewId { get; set; }

        [ForeignKey("MidTermReviewId")]
        public MidTermReview? MidTermReview { get; set; }

        [Required]
        public int KraNumber { get; set; } // 1, 2, 3, etc.

        [Required]
        public string KraDescription { get; set; } = string.Empty;

        [Required]
        [Range(1, 100, ErrorMessage = "Weight must be between 1 and 100")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Weight { get; set; }

        // --- Part C1: Motivation Fields ---
        public string AchievementStandard { get; set; } = string.Empty;
        public string SupervisorComments { get; set; } = string.Empty;

        // --- Part C2: Ratings (1 to 4 Scale) ---
        [Range(1, 4, ErrorMessage = "Rating must be between 1 and 4")]
        public int OwnRating { get; set; }

        [Range(1, 4, ErrorMessage = "Rating must be between 1 and 4")]
        public int SupervisorRating { get; set; }

        [Range(1, 4, ErrorMessage = "Rating must be between 1 and 4")]
        public int AgreedRating { get; set; }
    }
}
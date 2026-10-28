using System.ComponentModel.DataAnnotations;

namespace PMDSSystems.Models
{
    public class ChangePasswordViewModel
    {
        public string UserId { get; set; } = string.Empty;

        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
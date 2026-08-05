using System.ComponentModel.DataAnnotations;

namespace PMDSSystems.Models
{
    public class ChangePasswordViewModel
    {
        public string UserId { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        public string NewPassword { get; set; } = "";

        [Required]
        [Compare("NewPassword")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = "";
    }
}
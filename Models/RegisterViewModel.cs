using System.ComponentModel.DataAnnotations;

namespace PMDSSystems.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Persal Number")]
        public string PersalNumber { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        public string Password { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = "";
    }
}
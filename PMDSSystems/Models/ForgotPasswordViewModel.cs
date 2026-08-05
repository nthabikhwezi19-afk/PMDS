using System.ComponentModel.DataAnnotations;

namespace PMDSSystems.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
    }
}
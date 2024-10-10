using System.ComponentModel.DataAnnotations;

namespace TamsApi.InputModels
{
    public class Login
    {
        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

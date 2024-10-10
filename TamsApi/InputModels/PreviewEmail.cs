using System.ComponentModel.DataAnnotations;

namespace TamsApi.InputModels
{
    public class PreviewEmail
    {
        [Required]
        [EmailAddress]
        public string To { get; set; }

        [EmailAddress]
        public string Cc { get; set; }
    }
}

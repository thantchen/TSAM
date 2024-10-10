using System.ComponentModel.DataAnnotations;

namespace TamsApi.InputModels
{
    public class NewUser
    {
        public NewUser()
        {
            Roles = new string[0];
        }

        [Required]
        [MaxLength(512)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(256)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(256)]
        public string LastName { get; set; }

        [Required]
        [MinLength(256)]
        public string UserName { get; set; }

        public string[] Roles { get; set; }
    }
}

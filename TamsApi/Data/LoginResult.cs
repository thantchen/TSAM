using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TamsApi.Data
{
    public class LoginResult
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IdentityUser User { get; set; }
        public IList<string> Roles { get; set; }
        public bool Succeeded { get => User != null; }
        public bool Failed { get => !Succeeded; }

    }
}

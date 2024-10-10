using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TamsApi.Models
{
    [Serializable]
    public partial class User
    {
        [NotMapped]
        public Role Role { get; set; }
        [NotMapped]
        public bool IsActive { get; set; }
    }
}

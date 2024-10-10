using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TamsApi.Models
{
    public class LookupItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ReferenceId { get; set; }
    }
}

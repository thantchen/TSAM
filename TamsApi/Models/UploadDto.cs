using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TamsApi.Models
{
    public class UploadDto
    {
        public int FileRepositoryId { get; set; }
        public long? ParsedCount { get; set; }
        public string File { get; set; }
        public DateTime? UploadedDate { get; set; }
        public string UploadedBy { get; set; }
        public long? Size { get; set; }

    }
}

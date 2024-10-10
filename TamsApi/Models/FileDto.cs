using System;
using System.Collections.Generic;

namespace TamsApi.Models
{
    public partial class FileDto
    {
        public int ReferenceId { get; set; }
        public int Id { get; set; }
        public string File { get; set; }
        public int Size { get; set; } 
        public string UploadedBy { get; set; }
        public DateTime UploadedDate { get; set; }
    }
}

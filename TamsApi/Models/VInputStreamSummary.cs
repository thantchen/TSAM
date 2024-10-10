using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TamsApi.Data;

namespace TamsApi.Models
{
    public class VInputStreamSummary
    {
        public VInputStreamSummary()
        {
        }

        [Key]
        public int FileTypeId { get; set; }
        public int? FileRepositoryId { get; set; }
        public string FileTypeName { get; set; }
        public string FileName { get; set; }
        public DateTime? UploadedDate { get; set; }
        public string? UploadedBy { get; set; }
        public int TotalCount { get; set; }
    }

}

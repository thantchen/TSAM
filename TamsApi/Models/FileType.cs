using System;
using System.Collections.Generic;

namespace TamsApi.Models
{
    public partial class FileType
    {
        public FileType()
        {
            FileRepository = new HashSet<FileRepository>();
        }

        public int FileTypeId { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<FileRepository> FileRepository { get; set; }
    }
}

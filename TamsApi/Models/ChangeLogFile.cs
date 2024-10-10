using System;
using System.Collections.Generic;

namespace TamsApi.Models
{
    public partial class ChangeLogFile
    {
        public int ChangeLogFileId { get; set; }
        public int ChangeLogId { get; set; }
        public int FileRepositoryId { get; set; }

        public virtual ChangeLog ChangeLog { get; set; }
        public virtual FileRepository FileRepository { get; set; }
    }
}

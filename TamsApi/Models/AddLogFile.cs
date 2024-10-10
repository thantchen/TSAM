using System;
using System.Collections.Generic;

namespace TamsApi.Models
{
    public partial class AddLogFile
    {
        public int AddLogFileId { get; set; }
        public int AddLogId { get; set; }
        public int FileRepositoryId { get; set; }

        public virtual AddLog AddLog { get; set; }
        public virtual FileRepository FileRepository { get; set; }
    }
}

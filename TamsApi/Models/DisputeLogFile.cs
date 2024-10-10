using System;
using System.Collections.Generic;

namespace TamsApi.Models
{
    public partial class DisputeLogFile
    {
        public int DisputeLogFileId { get; set; }
        public int DisputeLogId { get; set; }
        public int FileRepositoryId { get; set; }

        public virtual DisputeLog DisputeLog { get; set; }
        public virtual FileRepository FileRepository { get; set; }
    }
}

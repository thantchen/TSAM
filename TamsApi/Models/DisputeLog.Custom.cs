using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TamsApi.Models
{
    [Serializable]
    public partial class DisputeLog : ITrackCreated, ITrackModified
    {
        [NotMapped]
        public string DisputeTypeText { get; set; }
        [NotMapped]
        public string DisputeDiscrepancyText { get; set; }
        [NotMapped]
        public string SubmittedByUserText { get; set; }
        [NotMapped]
        public string RequestedByUserText { get; set; }
        [NotMapped]
        public string CreatedUserText { get; set; }
        [NotMapped]
        public string LastModifiedUserText { get; set; }
        [NotMapped]
        public int AttachmentCount { get; set; }
    }
}

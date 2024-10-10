using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TamsApi.Models
{
    [Serializable]
    public partial class AddLog : ITrackCreated, ITrackModified
    {
        [NotMapped]
        public string ChangeLogTypeText { get; set; }
        [NotMapped]
        public string ChangeLogStatusText { get; set; }
        [NotMapped]
        public string SubmittedByUserText { get; set; }
        [NotMapped]
        public string RequestedByUserText { get; set; }
        [NotMapped]
        public string CreatedUserText { get; set; }
        [NotMapped]
        public string LastModifiedUserText { get; set; }
        [NotMapped]
        public string AddTypeText { get; set; }
        [NotMapped]
        public int AttachmentCount { get; set; }
    }
}

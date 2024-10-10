using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TamsApi.Models
{
    public partial class TsaSchedule : ITrackCreated, ITrackModified, ITrackRevisions
    {
        public string Status
        {
            get
            {
                var status = "";
                if (ActiveEndDate.HasValue && OriginalEndDate.HasValue && ActiveEndDate.Value.Date < OriginalEndDate.Value.Date)
                    status = "Cancelled";
                else if (ActiveEndDate.HasValue && OriginalEndDate.HasValue &&
                        ((ActiveEndDate.Value.Date <= DateTime.Today && ActiveEndDate.Value.Date == OriginalEndDate.Value.Date) || (OriginalEndDate.Value.Date <= ActiveEndDate.Value.Date && ActiveEndDate.Value.Date <= DateTime.Today)))
                    status = "Complete";
                else
                    status = "Active";

                return status;
            }
        }
    }
}

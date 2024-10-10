using System.Collections.Generic;

namespace TamsEmail.Views.Templates
{
    public class ChangeRequestSubmittedViewModel : RequestSubmittedViewModel
    {
        public ChangeRequestSubmittedViewModel()
        {
            LogNumbers = new List<TsaLogNumber>();
        }

        public override Dictionary<string, string> RequestDetails()
        {
            return new Dictionary<string, string>
            {
                { "TSA ID", TsaId },
                { "TSA Sub-ID", DelimitedSubIds() },
                { "Change Date", RequestDate },
                { "Change Type", RequestType },
                { "Status", Status },
                { "Requested By", RequestedBy },
                { "Submitted By", SubmittedBy },
                { "Attachment(s)", string.IsNullOrWhiteSpace(Attachments) ? "(none)" : Attachments }
            };
        }
    }
}

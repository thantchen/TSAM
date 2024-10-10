using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace TamsEmail.Views.Templates
{
    public class TsaLogNumber
    {
        public TsaLogNumber() { }
        public TsaLogNumber(string id, string tsaSubId)
        {
            Id = id;
            TsaSubId = tsaSubId;
        }

        public string Id { get; set; }
        public string TsaSubId { get; set; }
    }

    public class AddRequestSubmittedViewModel : RequestSubmittedViewModel
    {
        public AddRequestSubmittedViewModel()
        {
            LogNumbers = new List<TsaLogNumber>();
        }

        public override Dictionary<string, string> RequestDetails()
        {
            return new Dictionary<string, string>
            {
                { "Type of Add", AddType },
                { "TSA ID", String.IsNullOrEmpty(TsaId) ? "TBD" : TsaId },
                { "TSA Sub-ID", String.IsNullOrEmpty(DelimitedSubIds()) ? "TBD" : TsaId },
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

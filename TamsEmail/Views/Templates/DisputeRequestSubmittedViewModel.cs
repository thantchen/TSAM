using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace TamsEmail.Views.Templates
{
    public class DisputeRequestSubmittedViewModel : RequestSubmittedViewModel
    {
        public DisputeRequestSubmittedViewModel()
        {
            LogNumbers = new List<TsaLogNumber>();
        }

        public string InvoiceNumber { get; set; }

        public string Discrepancy { get; set; }

        public string ServicePeriod { get; set; }

        public override Dictionary<string, string> RequestDetails()
        {
            return new Dictionary<string, string>
            {
                { "TSA ID", TsaId },
                { "TSA Sub-ID", DelimitedSubIds() },
                { "Submission Date", RequestDate },
                { "Dispute Type", RequestType },
                { "Invoice #", InvoiceNumber },
                { "Requested By", RequestedBy },
                { "Submitted By", SubmittedBy },
                { "Discrepancy", Discrepancy },
                { "Service Period", ServicePeriod },
                { "Attachment(s)", string.IsNullOrWhiteSpace(Attachments) ? "(none)" : Attachments }
            };
        }
    }
}

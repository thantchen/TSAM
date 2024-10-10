using System;
using System.Collections.Generic;
using System.Linq;

namespace TamsEmail.Views.Templates
{
    public abstract class RequestSubmittedViewModel
    {
        public string Salutation { get; set; }

        public string TsaId { get; set; }

        public string RequestDate { get; set; }

        public string RequestType { get; set; }

        public string Status { get; set; }

        public string RequestedBy { get; set; }

        public string SubmittedBy { get; set; }

        public string Comments { get; set; }

        public string Attachments { get; set; }

        public string SupportEmailAddress { get; set; }
        
        public string ToEmailAddress { get; set; }

        public virtual List<TsaLogNumber> LogNumbers { get; set; }

        public abstract Dictionary<string, string> RequestDetails();

        public string DelimitedSubIds()
        {
            if (LogNumbers == null) return null;

            return string.Join("; ", LogNumbers.Select(l => l.TsaSubId?.Trim()));
        }

        public string AddType { get; set; }
    }
}

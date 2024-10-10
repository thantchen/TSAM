namespace TamsEmail.Views.Templates
{
    public abstract class StatusChangeViewModel
    {
        public string Salutation { get; set; }

        public string Id { get; set; }

        public string CurrentStatus { get; set; }

        public string TsaSubId { get; set; }

        public string TsaId { get; set; }

        public string Comments { get; set; }

        public string SupportEmailAddress { get; set; }

        public string ToEmailAddress { get; set; }
        public string AddType { get; set; }
        public string RequestType { get; set; }
    }
}

namespace TamsApi.Core.Email
{
    public class SmtpOptions
    {
        public const int DefaultPort = 25;
        public const string DefaultFrom = "tams-support@ankura.com";
        public const string DefaultSupportEmail = "tams-support@ankura.com";

        public string Server { get; set; }
        public int Port { get; set; } = DefaultPort;
        public string FromAddress { get; set; } = DefaultFrom;

        public string SupportAddress { get; set; } = DefaultSupportEmail;

        public bool EnableSsl { get; set; }
    }
}

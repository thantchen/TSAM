using System.Net.Mail;

namespace TamsApi.Core.Email
{
    public interface ISmtpClientBuilder
    {
        ISmtpClient Build(SmtpOptions options);
    }

    public class SmtpClientBuilder : ISmtpClientBuilder
    {
        public ISmtpClient Build(SmtpOptions options)
        {
            var client = new SmtpClient(options.Server, options.Port);
            client.EnableSsl = options.EnableSsl;
            return new SmtpClientWrapper(client);
        }
    }
}

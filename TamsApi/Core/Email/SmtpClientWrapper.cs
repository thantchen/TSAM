using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace TamsApi.Core.Email
{
    public interface ISmtpClient : IDisposable
    {
        string Host { get; }
        int Port { get; }
        void Send(MailMessage message);
    }

    // Wrapper class around SMTP Client to support testing. Super lame, but the SmtpClient
    // has no virtual members to override or mock...
    public class SmtpClientWrapper : ISmtpClient
    {
        private readonly SmtpClient _client;

        public string Host { get => _client.Host; }
        public int Port { get => _client.Port; }

        public SmtpClientWrapper(string server, int port) : this(new SmtpClient(server, port))
        {
        }

        public SmtpClientWrapper(SmtpClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            _client = client;
        }

        public void Send(MailMessage message)
        {
            _client.Send(message);
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}

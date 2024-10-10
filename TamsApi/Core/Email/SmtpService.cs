using Microsoft.Extensions.Options;
using System.Net.Mail;
using TamsApi.InputModels;
using TamsEmail.Templates;

namespace TamsApi.Core.Email
{
    public interface ISmtpService
    {
        MailMessage Send<TViewModel>(EmailRequest<TViewModel> request);
    }

    public class SmtpService : ISmtpService
    {
        private readonly IEmailRenderEngine _engine;
        private readonly ISmtpClientBuilder _builder;
        private readonly SmtpOptions _options;

        public SmtpService(
            IEmailRenderEngine engine,
            ISmtpClientBuilder builder,
            IOptions<SmtpOptions> optBuilder)
        {
            _engine = engine;
            _builder = builder;
            _options = optBuilder.Value;
        }

        public MailMessage Send<TViewModel>(EmailRequest<TViewModel> request)
        {
            string environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environment != null && environment.ToLower() != "production")
            {
                request.Subject = $"{environment}: {request.Subject}";
            }

            var contentTask = _engine.RenderViewToStringAsync(request.Template, request.Model);
            contentTask.Wait();
            var message = new MailMessage(
                _options.FromAddress,
                request.To,
                request.Subject,
                contentTask.Result);

            message.IsBodyHtml = true;
            if (request.Cc.IsPresent())
            {
                message.CC.Add(request.Cc);
            }
            message.Bcc.Add(_options.SupportAddress);

            using var smtpClient = _builder.Build(_options);
            smtpClient.Send(message);
            return message;
        }
    }
}

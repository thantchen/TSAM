using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TamsEmail.Templates;

namespace TamsApi.Core.Email
{
    public static class DependencyRegistry
    {
        public static IServiceCollection ConfigureSmtpOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SmtpOptions>(configuration.GetSection("SmtpSettings"));
            return services;
        }

        public static IServiceCollection AddEmailServices(this IServiceCollection services)
        {
            services.AddTransient<ISmtpClientBuilder, SmtpClientBuilder>();
            services.AddTransient<ISmtpService, SmtpService>();
            services.AddTransient<IEmailRenderEngine, RazorViewToStringRenderer>();
            return services;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;

namespace TamsApi.Data
{
    public static class RepositoryRegistry
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITsaRepository, TsaRepository>();
            services.AddScoped<IDataFileRepository, DataFileRepository>();
            services.AddScoped<ILookupRepository, LookupRepository>();
            services.AddScoped<IInputRepository, InputRepository>();
            services.AddScoped<IDataRepository, DataRepository>();
            services.AddScoped<IEmailRepository, EmailRepository>();
            services.AddScoped<ISettingsRepository, SettingsRepository>();
            return services;
        }
    }
}

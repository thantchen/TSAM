using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TamsApi.InputModels;

namespace TamsApi.Data.Seeds
{
    public class Users
    {
        private readonly IUserRepository _repository;
        private readonly UserSeedOptions _seedOptions;
        private readonly ILogger _logger;

        public Users(IUserRepository repository, IOptions<SeedOptions> options, ILogger logger)
        {
            _repository = repository;
            _seedOptions = options.Value.UsersSeed;
            _logger = logger;
        }

        public async Task Seed()
        {
            var adminUser = new NewUser
            {
                Email = _seedOptions.AdminEmail,
                UserName = _seedOptions.AdminUserName,
                FirstName = "Tams",
                LastName = "Administrator",
                Roles = new[] { "admin" }
            };

            if (string.IsNullOrWhiteSpace(adminUser.UserName))
            {
                _logger.LogInformation("User seed skipped for missing user name configuration value.");
                return;
            }

            if (_repository.Exists(adminUser.UserName))
            {
                _logger.LogInformation($"User already exists: {adminUser.UserName}");
                return;
            }

            await _repository.AddUserAsync(adminUser);
        }
    }
}

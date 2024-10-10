using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using TamsApi.Core;
using TamsApi.InputModels;
using TamsApi.Models;
using TsaModels=TsaData.Models;

namespace TamsApi.Data
{
    public class UserRepository : IUserRepository
    {
        public const string DefaultRole = "personnel";
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly ISystemClock _clock;
        private readonly ILogger<User> _logger;
        private readonly IDbContextFactory<TsaModels.TsaContext> _contextFactory;
        private TsaModels.TsaContext _dataContext;

        public UserRepository(ApplicationContext context, IDbContextFactory<TsaModels.TsaContext> contextFactory,
            IMapper mapper,
            ISystemClock clock,
            ILogger<User> logger)
        {
            _context = context;
            _mapper = mapper;
            _clock = clock;
            _logger = logger;
            _contextFactory = contextFactory;
            _dataContext = _contextFactory.CreateDbContext();
        }

        public async Task<User> AddUserAsync(NewUser input)
        {
            var user = _mapper.Map<User>(input);
            user.CreatedDate = _clock.UtcNow.UtcDateTime;
            var roles = input.Roles.Blank() ? new[] { DefaultRole } : input.Roles;
            var enabledRoles = _context.Roles.Where(r => r.Disabled == false && roles.Contains(r.Name));
            user.UserRoles.AddRange(enabledRoles.Select(r => new UserRole { Role = r }));
            _context.Users.Add(user);
            var saveResult = await _context.SaveChangesAsync();
            if (saveResult > 0)
            {
                return user;
            }

            _logger.LogWarning($"Failed to add new user: {input.UserName}");
            return null;
        }

        public bool Exists(string userName, bool ignoreDisabled = false)
        {
            if (ignoreDisabled)
            {
                return _context.Users.Any(u => u.UserName == userName && u.Disabled == false);
            }

            //var user = _dataContext.Users.Any(u => u.UserName == userName);
            return _context.Users.Any(u => u.UserName == userName);
            //return true;
        }

        public User Find(string userName, bool includeDisabled = false)
        {
            var userWithRoleQuery = _context.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role);
            if (includeDisabled)
            {
                return userWithRoleQuery.FirstOrDefault(u => u.UserName == userName);
            }

            return userWithRoleQuery.FirstOrDefault(u => u.UserName == userName && u.Disabled == false);
        }

        public User FindByEmail(string email)
        {
            return _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefault(u => u.Email == email && u.Disabled == false);
        }
    }
}

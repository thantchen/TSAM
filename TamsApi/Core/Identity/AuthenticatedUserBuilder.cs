using Microsoft.AspNetCore.Http;
using System.Security;

namespace TamsApi.Core.Identity
{
    public interface IAuthenticatedUserBuilder
    {
        AuthenticatedUser Build();
    }

    public class AuthenticatedUserBuilder : IAuthenticatedUserBuilder
    {
        private readonly IHttpContextAccessor _httpAccessor;

        public AuthenticatedUserBuilder(IHttpContextAccessor httpAccessor)
        {
            _httpAccessor = httpAccessor;
        }

        public AuthenticatedUser Build()
        {
            var principal = _httpAccessor.HttpContext.User;

            if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated)
            {
                throw new SecurityException("User is not authenticated.");
            }

            var authUser = new AuthenticatedUser(principal);
            if (authUser.UserId == default)
            {
                throw new SecurityException("User authenticated without sub claim.");
            }
            return authUser;
        }
    }
}

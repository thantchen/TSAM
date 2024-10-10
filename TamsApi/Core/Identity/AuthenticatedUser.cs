using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using TamsApi.Models;

namespace TamsApi.Core.Identity
{
    /// <summary>
    /// Wrapper class to make it easier to get authenticated user details from the controller.
    /// Instances are typically past to service/repository classes from the controller via 
    /// parameter injection rather than relying on global state and tying our DbContext to 
    /// MVC infrastructure.
    /// </summary>
    public class AuthenticatedUser
    {
        public long UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string[] Roles { get; set; }

        public AuthenticatedUser(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal),
                    "Cannot create authenticated user from null principal");
            }

            long.TryParse(FromClaim(principal, JwtRegisteredClaimNames.Sub), out var userId);
            UserId = userId;
            Email = FromClaim(principal, JwtRegisteredClaimNames.Email);
            FirstName = FromClaim(principal, JwtRegisteredClaimNames.GivenName) ?? "Unknown";
            LastName = FromClaim(principal, JwtRegisteredClaimNames.FamilyName) ?? "Unknown";
            Roles = LoadRoles(principal);
        }

        private AuthenticatedUser()
        {
        }

        private string FromClaim(ClaimsPrincipal principal, string type)
        {
            return principal.Claims.FirstOrDefault(c => c.Type == type)?.Value;
        }

        public static AuthenticatedUser FromUser(User user)
        {
            return new AuthenticatedUser
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = user.UserRoles.Select(ur => ur.Role.Name).ToArray()
            };
        }

        private static string[] LoadRoles(ClaimsPrincipal principal)
        {
            return principal.Claims
                .Where(c => c.Type == ClaimsIdentity.DefaultRoleClaimType)
                .Select(c => c.Value).ToArray();
        }
    }
}

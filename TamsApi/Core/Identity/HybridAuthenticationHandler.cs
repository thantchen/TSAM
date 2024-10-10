using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using NPOI.OpenXmlFormats.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TamsApi.Data;
using TamsApi.Models;

namespace TamsApi.Core.Identity
{
    public class HybridAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string SchemeName = "WindowsHybrid";

        private readonly IUserRepository _repository;

        public HybridAuthenticationHandler(
            IUserRepository userRepository,
            IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _repository = userRepository;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var windowsPrincipal = Context.User as WindowsPrincipal;
            if (windowsPrincipal == null)
            {
                Logger.LogError("No windows principal");
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            var identity = (WindowsIdentity)windowsPrincipal.Identity;
            var user = _repository.Find(identity.Name, includeDisabled: true);
            if (user == null || user.Disabled)
            {
                ClearAuthenticatedWindowsPrincipal();
                var reason = user == null ? "unregistered" : "disabled";
                var message = $"Cannot authenticate {reason} users: {identity.Name}";
                Logger.LogError(message);
                return Task.FromResult(
                    AuthenticateResult.Fail(message));
            }
            var claims = BuildClaims(user);
            var hybridIdentity = new ClaimsIdentity(claims, SchemeName);
            windowsPrincipal.AddIdentity(hybridIdentity);
            var ticket = new AuthenticationTicket(windowsPrincipal, SchemeName);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        // Clear the windows principal to ensure user is not authenticated.
        private void ClearAuthenticatedWindowsPrincipal() => Context.User = null;

        public static List<Claim> BuildClaims(User user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            // Add Sub in case Microsoft stops using NameIdentifier in the future.
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName));
            claims.Add(new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName));

            var enabledUrs = user.UserRoles.Where(ur => ur.Role.Disabled == false);
            var roleClaims = enabledUrs.Select(
                ur => new Claim(ClaimsIdentity.DefaultRoleClaimType, ur.Role.Name));
            claims.AddRange(roleClaims);
            return claims;
        }
    }
}

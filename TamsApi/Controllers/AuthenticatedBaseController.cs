using Microsoft.AspNetCore.Mvc;
using TamsApi.Core.Identity;

namespace TamsApi.Controllers
{
    public abstract class AuthenticatedControllerBase : ControllerBase
    {
        private AuthenticatedUser _user;
        private readonly IAuthenticatedUserBuilder _builder;
        
        public AuthenticatedUser CurrentUser
        {
            get
            {
                return _user ??= _builder.Build();
            }
        }

        protected AuthenticatedControllerBase(IAuthenticatedUserBuilder builder)
        {
            _builder = builder;
        }
    }
}

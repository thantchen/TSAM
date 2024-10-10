using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TamsApi.Core.Identity;
using TamsApi.Models;

namespace TamsApi.Data
{
    public interface ISettingsRepository
    {
        Task<List<User>> GetUsersAsync();
        Task<User> GetUserAsync(long userId);
        Task<User> UpdateUserAsync(User userData, AuthenticatedUser user);
        Task<User> AddUserAsync(User userData, AuthenticatedUser user);
    }

}

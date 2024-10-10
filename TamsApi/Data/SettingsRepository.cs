using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using NPOI.OpenXmlFormats.Wordprocessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TamsApi.Core;
using TamsApi.Core.Identity;
using TamsApi.Models;
using TamsApi.Models.Lookups;

namespace TamsApi.Data
{
    public class SettingsRepository : ISettingsRepository
    {
        #region Member variables

        private ApplicationContext _context;

        #endregion Member variables

        #region Constructor

        public SettingsRepository(ApplicationContext context)
        {
            _context = context;
        }

        #endregion Constructor

        #region Public methods

        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                var result = await _context.UserRoles.AsNoTracking()
                                                     .Include(i => i.Role)
                                                     .Include(i => i.User)
                                                     .OrderBy(o => o.User.FirstName)
                                                     .ThenBy(o => o.User.LastName)
                                                     .Select(s => new User
                                                     {
                                                         Id = s.UserId,
                                                         UserName = s.User.UserName,
                                                         FirstName = s.User.FirstName,
                                                         LastName = s.User.LastName,
                                                         Email = s.User.Email, 
                                                         Role = s.Role,
                                                         IsActive = !s.User.Disabled
                                                     }).ToListAsync();

                return result;
            }
            catch (Exception exception)
            {
                // TODO: Add logging here
                throw;
            };
        }

        public async Task<User> GetUserAsync(long userId)
        {
            try
            {
                var result = await _context.UserRoles.AsNoTracking()
                                                     .Include(i => i.Role)
                                                     .Include(i => i.User)
                                                     .Where(c => c.UserId == userId)
                                                     .Select(s => new User
                                                     {
                                                         Id = s.UserId,
                                                         UserName = s.User.UserName,
                                                         FirstName = s.User.FirstName,
                                                         LastName = s.User.LastName,
                                                         Email = s.User.Email,
                                                         Role = s.Role,
                                                         IsActive = !s.User.Disabled
                                                     }).FirstOrDefaultAsync();

                return result;
            }
            catch (Exception exception)
            {
                // TODO: Add logging here
                throw;
            };
        }

        public async Task<User> UpdateUserAsync(User userData, AuthenticatedUser user)
        {
            try
            {
                var result = await _context.Users.Where(c => c.Id == userData.Id).FirstOrDefaultAsync();
                if (result != null)
                {
                    result.FirstName = userData.FirstName;
                    result.LastName = userData.LastName;
                    result.UserName = userData.UserName;
                    result.Email = userData.Email;
                    if (result.Disabled != !userData.IsActive)
                    {
                        result.DisabledAt = DateTime.UtcNow;
                    }
                    result.Disabled = !userData.IsActive;
                    result.LastModifiedDate = DateTime.UtcNow;

                    var userRole = await _context.UserRoles.Where(c => c.UserId == userData.Id).FirstOrDefaultAsync();
                    if (userRole != null)
                    {
                        if (userRole.RoleId != userData.Role.Id)
                        {
                            userRole.RoleId = userData.Role.Id;
                            await _context.SaveChangesAsync();
                        }
                    }

                    await _context.SaveChangesAsync();

                    result = await GetUserAsync(result.Id);
                }

                return result;
            }
            catch (Exception exception)
            {
                // TODO: Add logging here
                throw;
            };
        }

        public async Task<User> AddUserAsync(User userData, AuthenticatedUser user)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var result = new User();

                    result.FirstName = userData.FirstName;
                    result.LastName = userData.LastName;
                    result.UserName = userData.UserName;
                    result.Email = userData.Email;
                    result.Disabled = !userData.IsActive;
                    result.LastModifiedDate = DateTime.UtcNow;
                    result.CreatedDate = DateTime.UtcNow;

                    await _context.Users.AddAsync(result);
                    await _context.SaveChangesAsync();

                    if (userData.Role != null)
                    {
                        var userRole = new UserRole();
                        userRole.UserId = result.Id;
                        userRole.RoleId = userData.Role.Id;

                        await _context.UserRoles.AddAsync(userRole);
                        await _context.SaveChangesAsync();
                    }
                    transaction.Commit();

                    return await GetUserAsync(result.Id); 
                }
                catch (Exception)
                {
                    transaction.Rollback();

                    // TODO: Add logging here
                    throw;
                }
            }
        }

        #endregion Private methods
    }
}

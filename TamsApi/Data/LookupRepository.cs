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
using System.Threading.Tasks;
using TamsApi.Models;

namespace TamsApi.Data
{
    public class LookupRepository : ILookupRepository
    {
        #region Member variables

        private ApplicationContext _context;

        #endregion Member variables

        #region Constructor

        public LookupRepository(ApplicationContext context)
        {
            _context = context;
        }

        #endregion Constructor

        #region Public methods

        public async Task<List<TEntity>> GetLookupDataAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            try
            {
                var result = await _context.Set<TEntity>().Where(predicate)
                                                          .ToListAsync();

                return result;
            }
            catch (Exception exeption)
            {
                // TODO: Add logging here
                throw;
            };
        }

        public async Task<List<TEntity>> GetLookupDataAsync<TEntity>() where TEntity : class
        {
            try
            {
                var result = await _context.Set<TEntity>().ToListAsync();

                return result;
            }
            catch (Exception exeption)
            {
                // TODO: Add logging here
                throw;
            };
        }

        public async Task<List<LookupTsaDto>> GetTsaLookupDataAsync()
        {
            try
            {
                var result = await _context.TsaSchedule.AsNoTracking().OrderBy(o => o.TsaId).ThenBy(t => t.TsaSubId).Select(s => new LookupTsaDto
                {
                    TsaId = s.TsaId,
                    TsaSubId = s.TsaSubId
                }).ToListAsync();

                return result;
            }
            catch (Exception exeption)
            {
                // TODO: Add logging here
                throw;
            };
        }

        public async Task<List<UserDto>> GetUserLookupDataAsync()
        {
            try
            {
                var result = await _context.Users.AsNoTracking().OrderBy(o => o.Email).Select(s => new UserDto
                {
                    Id = s.Id,
                    Username = s.UserName,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email
                }).ToListAsync();

                return result;
            }
            catch (Exception exeption)
            {
                // TODO: Add logging here
                throw;
            };
        }

        public List<LookupItemDto> GetCurrencyData()
        {
            var currencies = new List<LookupItemDto>();

            currencies.Add(new LookupItemDto { Name = "CAD" });
            currencies.Add(new LookupItemDto { Name = "INR" });
            currencies.Add(new LookupItemDto { Name = "USD" });

            return currencies;
        }

        public async Task<List<VLogTypeStatus>> GetLogTypeStatus()
        {
            return await _context.VLogTypeStatus.ToListAsync();
        }

        #endregion Private methods
    }
}

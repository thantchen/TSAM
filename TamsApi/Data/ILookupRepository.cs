using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TamsApi.Models;

namespace TamsApi.Data
{
    public interface ILookupRepository
    {
        Task<List<TEntity>> GetLookupDataAsync<TEntity>() where TEntity : class;
        Task<List<TEntity>> GetLookupDataAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        Task<List<LookupTsaDto>> GetTsaLookupDataAsync();
        Task<List<UserDto>> GetUserLookupDataAsync();
        List<LookupItemDto> GetCurrencyData();
        Task<List<VLogTypeStatus>> GetLogTypeStatus();
    }

}

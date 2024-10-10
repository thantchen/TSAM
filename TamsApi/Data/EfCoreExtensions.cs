using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TamsApi.Data
{
    public static class EfCoreExtensions
    {
        public static bool IsModified<TModel>(this EntityEntry<TModel> entry) where TModel : class
        {
            return entry.State == EntityState.Added || entry.State == EntityState.Modified;
        }
    }
}

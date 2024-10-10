using System.Threading.Tasks;
using TamsApi.InputModels;
using TamsApi.Models;

namespace TamsApi.Data
{
    public interface IUserRepository
    {
        Task<User> AddUserAsync(NewUser input);
        bool Exists(string userName, bool ignoreDisabled = false);
        User Find(string userName, bool includeDisabled = false);
        User FindByEmail(string email);
    }
}

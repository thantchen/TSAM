using TamsApi.Models;

namespace TamsApi.Data
{
    public interface ITsaRepository
    {
        bool Exists(string tsaSubId);
    }
}

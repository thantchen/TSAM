using System.Threading.Tasks;

namespace TamsEmail.Templates
{
    public interface IEmailRenderEngine
    {
        Task<string> RenderViewToStringAsync<TModel>(string viewName, TModel model);
    }
}

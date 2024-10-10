using System.Collections.Generic;

namespace TamsApi.Core.Excel
{
    public interface IExcelRowValidator<TModel> where TModel : class
    {
        List<string> Validate(TModel model);
    }
}

using System.IO;

namespace TamsApi.Core.Excel
{
    public interface ISheetParser<TModel> where TModel : class
    {
        ParseResult<TModel> Parse(Stream excelStream);
    }
}

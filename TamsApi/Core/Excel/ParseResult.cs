using System.Collections.Generic;

namespace TamsApi.Core.Excel
{
    public class ParseResult<TModel> where TModel : class
    {
        public List<TModel> ParsedRows { get; set; } = new List<TModel>();
        public List<FailedRow> FailedRows { get; set; } = new List<FailedRow>();
        public bool Succeeded { get => FailedRows.Blank(); }
        public bool Failed { get => !Succeeded; }

        public ParseResult<TModel> Add(TModel model)
        {
            ParsedRows.Add(model);
            return this;
        }

        public ParseResult<TModel> Add(long rowIndex, IDictionary<string, object> parsedCells, List<string> errors)
        {
            FailedRows.Add(new FailedRow(
                rowIndex,
                parsedCells,
                new List<string>(errors)));
            return this;
        }
    }

    public readonly struct FailedRow
    {
        // The index value the user would see in Excel. For example,
        // the value for RowIndex would be 5 if there was a problem in cell C5.
        public long RowIndex { get; }

        // Raw cell values
        public IDictionary<string, object> ParsedCells { get; }

        public List<string> Errors { get; }

        public FailedRow(long rowIndex, IDictionary<string, object> parsedCells, List<string> errors) : this()
        {
            RowIndex = rowIndex;
            ParsedCells = parsedCells;
            Errors = errors;
        }
    }
}

using System;

namespace TamsApi.Core.Excel
{
    public class ExcelParseException : Exception
    {
        public ExcelParseException(string message) : base(message)
        {
        }

        public ExcelParseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

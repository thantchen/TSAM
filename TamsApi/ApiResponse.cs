using System.Collections.Generic;
using System.Text.Json.Serialization;
using TamsApi.Core;

namespace TamsApi
{
    public class ApiResponse<TData>
    {
        public ApiResponse(TData data, List<string> messages = null)
        {
            Data = data;
            Messages = messages ?? new List<string>();
            Errors = new List<string>();
        }

        public ApiResponse(List<string> errors)
        {
            Errors = errors;
        }

        public ApiResponse(params string[] errors)
        {
            Errors = new List<string>(errors);
        }

        public TData Data { get; private set; }
        public List<string> Messages { get; private set; }
        public List<string> Errors { get; set; }

        [JsonIgnore]
        public bool Failed => Errors != null && Errors.Count > 0;
    }

    public class ApiResponse
    {
        public static ApiResponse<TModel> Failed<TModel>(TModel data, List<string> errors)
        {
            return new ApiResponse<TModel>(data) { Errors = errors };
        }
    }
}

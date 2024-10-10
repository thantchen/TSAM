namespace TamsApi.Core
{
    public class ValidResult<TModel>
    {
        public TModel Value { get; internal set; }
        public bool Succeeded { get; internal set; }
        public bool Failed { get => !Succeeded; }
    }

    public static class ValidResult
    {   
        public static ValidResult<TValue> Success<TValue>(TValue model)
        {
            return new ValidResult<TValue> { Value = model, Succeeded = true };
        }

        public static ValidResult<TValue> Fail<TValue>()
        {
            return new ValidResult<TValue> { Succeeded = false };
        }
    }
}

namespace TamsApi.InputModels
{
    public class EmailRequest<TViewModel>
    {
        public string Template { get; set; }
        public TViewModel Model { get; set; }

        public string To { get; set; }
        public string Cc { get; set; }

        public string Subject { get; set; }
    }
}

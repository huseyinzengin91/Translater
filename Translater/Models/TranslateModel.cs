namespace Translater.Models
{
    public class TranslateModel
    {
        public string LanguageSource { get; set; }

        public string LanguageDestinaton { get; set; }

        public string Source { get; set; }

        public string Destination { get; set; }

        public bool IsSuccess { get; set; }

        public string ErrorText { get; set; }
    }
}
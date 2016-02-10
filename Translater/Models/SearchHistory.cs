namespace Translater.Models
{
    public class SearchHistory
    {
        public long ID { get; set; }

        public string Keyword { get; set; }

        public int Count { get; set; }

        public System.DateTime LastDate { get; set; }
    }
}
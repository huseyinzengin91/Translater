using System.Collections.Generic;

namespace Translater.Models
{
    public class ResponseModel
    {
        public int Code { get; set; }

        public string Lang { get; set; }

        public List<string> Text { get; set; }

        public string Message { get; set; }
    }
}
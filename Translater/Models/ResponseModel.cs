#region Using

using System.Collections.Generic;

#endregion Using

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
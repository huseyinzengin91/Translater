#region Using

using System.Collections.Generic;
using System.Web.Mvc;

#endregion Using

namespace Translater.Models
{
    public class IndexModel
    {
        public List<SelectListItem> LanguageList { get; set; }
    }
}
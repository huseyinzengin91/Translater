#region Using

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Translater.Models;

#endregion Using

namespace Translater.Controllers
{
    public class HomeController : Controller
    {
        #region Variable

        private ModelContext modelContext = new ModelContext();
        private string uri = "https://translate.yandex.net/api/v1.5/tr.json/translate?key=trnsl.1.1.20150122T203315Z.bf114d5768d7abcc.2e8030df70953e98448b2b87e2159bc9a4ca7be2&lang={0}-{1}&text={2}";

        #endregion Variable

        #region Public Methods

        public ActionResult Index()
        {
            IndexModel indexModel = new IndexModel();
            indexModel.LanguageList = this.GetLanguageList();

            return View(indexModel);
        }

        public async Task<JsonResult> Translate(TranslateModel translateModel)
        {
            HttpClient client = new HttpClient();

            uri = string.Format(uri, translateModel.LanguageSource, translateModel.LanguageDestinaton, translateModel.Source);

            string translateResponse = await client.GetStringAsync(uri);

            ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(translateResponse);

            if (string.IsNullOrEmpty(responseModel.Message) && responseModel.Code == 200)
            {
                translateModel.Destination = string.Join(",", responseModel.Text);
                translateModel.IsSuccess = true;
                Upsert(translateModel.Source);
            }
            else
            {
                translateModel.ErrorText = responseModel.Message;
            }

            return Json(translateModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLastRecords()
        {
            List<SearchHistory> searhList = modelContext.SearchHistory.OrderByDescending(z => z.ID).Take(10).ToList();

            return Json(searhList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTopRecords()
        {
            List<SearchHistory> searhList = modelContext.SearchHistory.OrderByDescending(z => z.Count).Take(10).ToList();

            return Json(searhList, JsonRequestBehavior.AllowGet);
        }

        #endregion Public Methods

        #region Private Methods

        private void Upsert(string keyword)
        {
            SearchHistory existKeyword = modelContext.SearchHistory.FirstOrDefault(z => z.Keyword.Equals(keyword));

            if (existKeyword == null)
            {
                existKeyword = new SearchHistory();
                existKeyword.Keyword = keyword;
                existKeyword.Count = 1;
                existKeyword.LastDate = DateTime.Now;
                modelContext.SearchHistory.Add(existKeyword);
            }
            else
            {
                existKeyword.Count += 1;
                existKeyword.LastDate = DateTime.Now;
            }

            modelContext.SaveChanges();
        }

        private List<SelectListItem> GetLanguageList()
        {
            List<SelectListItem> languageList = new List<SelectListItem>();

            languageList.Add(new SelectListItem()
            {
                Text = "TR",
                Value = "tr"
            });

            languageList.Add(new SelectListItem()
            {
                Text = "EN",
                Value = "en"
            });

            languageList.Add(new SelectListItem()
            {
                Text = "IT",
                Value = "it"
            });
            languageList.Add(new SelectListItem()
            {
                Text = "FR",
                Value = "fr"
            });

            return languageList;
        }

        #endregion Private Methods
    }
}
#region Using

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        private string apiUrl = string.Empty;

        #endregion Variable

        #region Public Methods

        #region Constructor

        public HomeController()
        {
            apiUrl = string.Join("", ConfigurationManager.AppSettings.Get("YandexUrl"), ConfigurationManager.AppSettings.Get("YandexKey"), "&lang={0}-{1}&text={2}");
        }

        #endregion Constructor

        public ActionResult Index()
        {
            IndexModel indexModel = new IndexModel();
            indexModel.LanguageList = this.GetLanguageList();

            return View(indexModel);
        }

        public async Task<JsonResult> Translate(TranslateModel translateModel)
        {
            HttpClient client = new HttpClient();

            apiUrl = string.Format(apiUrl, translateModel.LanguageSource, translateModel.LanguageDestinaton, translateModel.Source);

            string translateResponse = await client.GetStringAsync(apiUrl);

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
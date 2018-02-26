using System;
using System.Collections.Generic;
using System.Net.Http;
using AktuelListesi.Crawler.Interfaces;
using AktuelListesi.Crawler.Models;
using Newtonsoft.Json;

namespace AktuelListesi.Crawler
{
    public class CrawlerService : ICrawlerService
    {
        public List<CrawlerItem> GetAktuelPages(string categoryId)
        {
            var model = GetModel($"http://aktuel-urunlerim.com/aktuel1/api.php?cat_id={categoryId}");
            if (model == null) return new List<CrawlerItem>();
            return model.CompanyItems;
        }

        public List<CrawlerItem> GetCompanies()
        {
            var model = GetModel("http://aktuel-urunlerim.com/aktuel1/api.php");

            if (model == null) return new List<CrawlerItem>();
            return model.CompanyItems;
        }

        public List<CrawlerItem> GetLatest()
        {
            var model = GetModel("http://aktuel-urunlerim.com/aktuel1/api.php?latest_news=100");

            if (model == null) return new List<CrawlerItem>();
            return model.CompanyItems;
        }

        private CrawlerModel GetModel(string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                { 
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                    var requestTask = client.GetAsync(url);
                    requestTask.Wait();
                    var request = requestTask.Result;
                    
                    var responseTask = request.Content.ReadAsStringAsync();
                    responseTask.Wait();
                    return JsonConvert.DeserializeObject<CrawlerModel>(responseTask.Result);
                }
            }
            catch
            {
                return new CrawlerModel();
            }
        }
    }
}

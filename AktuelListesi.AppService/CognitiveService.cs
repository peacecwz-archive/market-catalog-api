using AktuelListesi.AppService.Interfaces;
using AktuelListesi.AppService.Models;
using AktuelListesi.Models.AppServices;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace AktuelListesi.AppService
{
    public class CognitiveService : ICognitiveService
    {
        public CognitiveServiceOptions ServiceOptions { get; set; }
        public CognitiveService(IOptions<CognitiveServiceOptions> serviceOptions)
        {
            this.ServiceOptions = serviceOptions?.Value;
        }

        public CognitiveService(CognitiveServiceOptions serviceOptions)
        {
            this.ServiceOptions = serviceOptions;
        }

        public CognitiveService() { }

        public string ReadTextFromImage(string ImageUrl)
        {
            try
            {
                if (ServiceOptions == null) return "";
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Ocp-Apim-Subscription-Key", ServiceOptions.ServiceKey);
                    using (StringContent content = new StringContent(JsonConvert.SerializeObject(new { url = ImageUrl }), Encoding.UTF8, "application/json"))
                    {
                        var requestTask = client.GetAsync($"{ServiceOptions.ServiceUrl}/ocr?language={ServiceOptions.Language}&detectOrientation=true");
                        requestTask.Wait();
                        var request = requestTask.Result;

                        var responseTask = request.Content.ReadAsStringAsync();
                        responseTask.Wait();
                        var model = JsonConvert.DeserializeObject<CognitiveServiceModel>(responseTask.Result);
                        string text = "";
                        foreach (var region in model.Regions)
                        {
                            foreach (var line in region.Lines)
                            {
                                foreach (var word in line.Words)
                                {
                                    text += word.Text;
                                }
                            }
                        }
                        return text;
                    }
                }
            }
            catch
            {
                return "";
            }
        }
    }
}

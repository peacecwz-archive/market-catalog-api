using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AktuelListesi.AppService.Interfaces;
using AktuelListesi.Crawler.Interfaces;
using AktuelListesi.Models.App;
using AktuelListesi.DataService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AktuelListesi.Data.Dtos;
using AktuelListesi.Crawler.Models;

namespace AktuelListesi.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class AppController : ApiController
    {
        private readonly IAktuelPageService aktuelPageService;
        private readonly IAktuelService aktuelService;
        private readonly ICompanyService companyService;
        private readonly IQueueService queueService;
        private readonly ICrawlerService crawlerService;
        private readonly IUploadService uploadService;
        private readonly ICognitiveService cognitiveService;
        public AppController(IAktuelPageService aktuelPageService,
                             IAktuelService aktuelService,
                             ICompanyService companyService,
                             IQueueService queueService,
                             ICrawlerService crawlerService,
                             IUploadService uploadService,
                             ICognitiveService cognitiveService)
        {
            this.aktuelPageService = aktuelPageService;
            this.aktuelService = aktuelService;
            this.companyService = companyService;
            this.queueService = queueService;
            this.crawlerService = crawlerService;
            this.uploadService = uploadService;
            this.cognitiveService = cognitiveService;
        }

        [HttpGet("latest")]
        public IActionResult Latest()
        {
            var model = new LatestModel();
            model.Aktuels = aktuelService.GetLatestAktuels();
            model.Companies = companyService.GetCompanies();
            return Ok(model);
        }

        [HttpGet("update")]
        public IActionResult Update()
        {
            var latestItems = crawlerService.GetLatest();
            aktuelService.DeactiveLatestAktuels();
            foreach (var latestItem in latestItems)
            {
                AddCrawlerItemToDb(latestItem);
            }
            return Ok();
        }

        [HttpGet("initialize")]
        public IActionResult Initialize()
        {
            Task.Run(() =>
            {
                var companies = crawlerService.GetCompanies();
                foreach (var company in companies)
                {
                    var aktuelNewsItems = crawlerService.GetAktuelPages(company.CategoryId);
                    foreach (var aktuelNews in aktuelNewsItems)
                    {
                        AddCrawlerItemToDb(aktuelNews);
                    }
                }
            });
            return Ok();
        }

        [HttpPost("analyzeImage")]
        public IActionResult AnalyzeImage([FromBody]AktuelPageDto aktuelPageDto)
        {
            aktuelPageDto.PageImageUrl = uploadService.UploadFile(aktuelPageDto.OriginalImageUrl);
            string content = cognitiveService.ReadTextFromImage(aktuelPageDto.PageImageUrl);
            aktuelPageDto.Content = content;
            aktuelPageDto = aktuelPageService.UpdateAktuelPage(aktuelPageDto);
            if (aktuelPageDto != null)
                return Ok(aktuelPageDto);
            return BadRequest();
        }

        #region Helpers

        void AddCrawlerItemToDb(CrawlerItem latestItem)
        {
            var companyDto = companyService.AddOrGetCompany(new Data.Dtos.CompanyDto()
            {
                Name = latestItem.CategoryName,
                ImageUrl = uploadService.UploadFile("http://aktuel-urunlerim.com/aktuel1/upload/" + latestItem.NewsImage),
                CategoryId = int.Parse(latestItem.CategoryId),
                IsActive = true
            });

            DateTime relaseDate = DateTime.Now;
            DateTime.TryParse(latestItem.NewsDate, out relaseDate);
            var aktuelDto = aktuelService.AddOrGetAktuel(new Data.Dtos.AktuelDto()
            {
                Name = latestItem.NewsHeading,
                ImageUrl = companyDto.ImageUrl,
                CompanyId = companyDto.Id,
                NewsId = int.Parse(latestItem.NewsId),
                ReleasedDate = relaseDate,
                ReleasedDateString = latestItem.NewsDate,
                IsActive = true,
                IsLatest = true,
                CreatedAt = DateTime.Now
            });

            if (!aktuelDto.IsLatest)
            {
                aktuelDto.IsLatest = true;
                aktuelService.UpdateAktuel(aktuelDto);
            }

            List<AktuelPageDto> aktuelPageDtos = new List<AktuelPageDto>();
            foreach (var page in latestItem.Links)
            {
                var aktuelPage = new Data.Dtos.AktuelPageDto()
                {
                    AktuelId = aktuelDto.Id,
                    OriginalImageUrl = page,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };
                aktuelPageDtos.Add(aktuelPage);
            }
            aktuelPageService.AddRange(aktuelPageDtos);
            aktuelPageDtos.Where(x => string.IsNullOrEmpty(x.Content)).ToList().ForEach(x => queueService.AddQueue(JsonConvert.SerializeObject(x)));
        }

        #endregion
    }
}

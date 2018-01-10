using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AktuelListesi.AppService.Interfaces;
using AktuelListesi.Crawler.Interfaces;
using AktuelListesi.Models.App;
using AktuelListesi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        public AppController(IAktuelPageService aktuelPageService,
                             IAktuelService aktuelService,
                             ICompanyService companyService,
                             IQueueService queueService,
                             ICrawlerService crawlerService)
        {
            this.aktuelPageService = aktuelPageService;
            this.aktuelService = aktuelService;
            this.companyService = companyService;
            this.queueService = queueService;
            this.crawlerService = crawlerService;
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
            foreach (var latestItem in latestItems)
            {
                var companyDto = companyService.AddOrGetCompany(new Data.Dtos.CompanyDto()
                {
                    Name = latestItem.CategoryName,
                    ImageUrl = latestItem.CategoryImage,
                    CategoryId = int.Parse(latestItem.CategoryId),
                    IsActive = true,
                    CreatedAt = DateTime.Now
                });

                var aktuelDto = aktuelService.AddOrGetAktuel(new Data.Dtos.AktuelDto()
                {
                    Name = latestItem.NewsHeading,
                    ImageUrl = latestItem.NewsImage,
                    CompanyId = companyDto.Id,
                    NewsId = int.Parse(latestItem.NewsId),
                    ReleasedDate = latestItem.NewsDate,
                    IsActive = true,
                    IsLatest = true
                });

                foreach (var page in latestItem.Links)
                {
                    aktuelPageService.AddOrGetAktuelPage(new Data.Dtos.AktuelPageDto()
                    {
                        AktuelId = aktuelDto.Id,
                        PageImageUrl = page,
                        IsActive = true
                    });
                }
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
                    var companyDto = companyService.AddOrGetCompany(new Data.Dtos.CompanyDto()
                    {
                        Name = company.CategoryName,
                        ImageUrl = company.CategoryImage,
                        CategoryId = int.Parse(company.CategoryId),
                        IsActive = true
                    });

                    var aktuelNewsItems = crawlerService.GetAktuelPages(company.CategoryId);
                    foreach (var aktuelNews in aktuelNewsItems)
                    {
                        var aktuelDto = aktuelService.AddOrGetAktuel(new Data.Dtos.AktuelDto()
                        {
                            Name = aktuelNews.NewsHeading,
                            ImageUrl = aktuelNews.NewsImage,
                            CompanyId = companyDto.Id,
                            NewsId = int.Parse(aktuelNews.NewsId),
                            ReleasedDate = aktuelNews.NewsDate,
                            IsActive = true,
                            IsLatest = true
                        });

                        foreach (var page in aktuelNews.Links)
                        {
                            aktuelPageService.AddOrGetAktuelPage(new Data.Dtos.AktuelPageDto()
                            {
                                AktuelId = aktuelDto.Id,
                                PageImageUrl = page,
                                IsActive = true
                            });
                        }
                    }
                }
            });
            return Ok();
        }


    }
}
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
        public AppController(IAktuelPageService aktuelPageService,
                             IAktuelService aktuelService,
                             ICompanyService companyService,
                             IQueueService queueService,
                             ICrawlerService crawlerService,
                             IUploadService uploadService)
        {
            this.aktuelPageService = aktuelPageService;
            this.aktuelService = aktuelService;
            this.companyService = companyService;
            this.queueService = queueService;
            this.crawlerService = crawlerService;
            this.uploadService = uploadService;
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

                var companyDto = companyService.AddOrGetCompany(new Data.Dtos.CompanyDto()
                {
                    Name = latestItem.CategoryName,
                    ImageUrl = uploadService.UploadFile("http://aktuel-urunlerim.com/aktuel1/upload/" + latestItem.NewsImage),
                    CategoryId = int.Parse(latestItem.CategoryId),
                    IsActive = true
                });

                var aktuelDto = aktuelService.AddOrGetAktuel(new Data.Dtos.AktuelDto()
                {
                    Name = latestItem.NewsHeading,
                    ImageUrl = companyDto.ImageUrl,
                    CompanyId = companyDto.Id,
                    NewsId = int.Parse(latestItem.NewsId),
                    ReleasedDate = latestItem.NewsDate,
                    IsActive = true,
                    IsLatest = true,
                    CreatedAt = DateTime.Now
                });


                if (!aktuelDto.IsLatest)
                {
                    aktuelDto.IsLatest = true;
                    aktuelService.UpdateAktuel(aktuelDto);
                }

                foreach (var page in latestItem.Links)
                {
                    aktuelPageService.AddOrGetAktuelPage(new Data.Dtos.AktuelPageDto()
                    {
                        AktuelId = aktuelDto.Id,
                        PageImageUrl = uploadService.UploadFile(page),
                        OriginalImageUrl = page,
                        IsActive = true,
                        CreatedAt = DateTime.Now
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
                    var aktuelNewsItems = crawlerService.GetAktuelPages(company.CategoryId);
                    foreach (var aktuelNews in aktuelNewsItems)
                    {
                        var companyDto = companyService.AddOrGetCompany(new Data.Dtos.CompanyDto()
                        {
                            Name = company.CategoryName,
                            ImageUrl = uploadService.UploadFile("http://aktuel-urunlerim.com/aktuel1/upload/" + aktuelNews.NewsImage),
                            CategoryId = int.Parse(company.CategoryId),
                            IsActive = true
                        });

                        var aktuelDto = aktuelService.AddOrGetAktuel(new Data.Dtos.AktuelDto()
                        {
                            Name = aktuelNews.NewsHeading,
                            ImageUrl = companyDto.ImageUrl,
                            OriginalImageUrl = "http://aktuel-urunlerim.com/aktuel1/upload/" + aktuelNews.NewsImage,
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
                                PageImageUrl = uploadService.UploadFile(page),
                                IsActive = true,
                                OriginalImageUrl = page
                            });
                        }
                    }
                }
            });
            return Ok();
        }


    }
}
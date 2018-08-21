using AktuelListesi.AppService.Interfaces;
using AktuelListesi.Crawler.Interfaces;
using AktuelListesi.Crawler.Models;
using AktuelListesi.Data.Dtos;
using AktuelListesi.Data.Tables;
using AktuelListesi.DataService;
using AktuelListesi.Models.AppServices;
using AktuelListesi.Repository;
using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AktuelListesi.Updater
{
    public class Functions
    {
        private readonly IMapper mapper;
        private readonly ICrawlerService crawlerService;
        private readonly IQueueService queueService;
        private readonly IUploadService uploadService;
        private readonly ICognitiveService cognitiveService;
        private readonly AktuelDbContext dbContext;
        public Functions(AktuelDbContext dbContext,
                        IQueueService queueService,
                        ICrawlerService crawlerService,
                        IUploadService uploadService,
                        ICognitiveService cognitiveService,
                        IMapper mapper)
        {
            this.queueService = queueService;
            this.crawlerService = crawlerService;
            this.uploadService = uploadService;
            this.cognitiveService = cognitiveService;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public void StartUpdateJob([QueueTrigger("aktuellistesi-update")]string message)
        {
            try
            {
                Console.WriteLine("StartUpdateJob is running");
                var latestItems = crawlerService.GetLatest();
                Console.WriteLine($"Getting Latest Aktuels. Count: {latestItems.Count}");
                var aktuels = dbContext.Aktuels.AsNoTracking().Where(x => x.IsActive && !x.IsDeleted && x.IsLatest == true).ToList();
                Console.WriteLine($"Getting Latest Aktuels on Database. Count: {aktuels.Count}");
                aktuels.ForEach(x => x.IsLatest = false);
                dbContext.Aktuels.UpdateRange(aktuels);
                dbContext.SaveChanges();
                Console.WriteLine("Updated all aktuels");
                foreach (var latestItem in latestItems)
                {
                    Console.WriteLine($"Processing {latestItem.CategoryName}");
                    AddCrawlerItemToDb(latestItem);
                    Console.WriteLine($"Process Completed {latestItem.CategoryName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Job Complated");
        }

        void AddCrawlerItemToDb(CrawlerItem latestItem)
        {
            try
            {
                var company = new Company()
                {
                    Name = latestItem.CategoryName,
                    ImageUrl = uploadService.UploadFile("http://aktuel-urunlerim.com/aktuel1/upload/" + latestItem.NewsImage),
                    CategoryId = int.Parse(latestItem.CategoryId),
                    IsActive = true
                };

                CreateOrUpdateCompany(company);

                Console.WriteLine($"Created {company.Name} Company");

                DateTime relaseDate = DateTime.Now;
                DateTime.TryParse(latestItem.NewsDate, out relaseDate);
                var aktuel = new Aktuel()
                {
                    Name = latestItem.NewsHeading,
                    ImageUrl = company.ImageUrl,
                    CompanyId = company.Id,
                    NewsId = int.Parse(latestItem.NewsId),
                    ReleasedDate = relaseDate,
                    ReleasedDateString = latestItem.NewsDate,
                    IsActive = true,
                    IsLatest = true,
                    CreatedAt = DateTime.Now
                };
                CreateOrUpdateAktuel(aktuel);
                Console.WriteLine($"Created {aktuel.Name} Aktuel");

                if (!aktuel.IsLatest)
                {
                    aktuel.IsLatest = true;
                    dbContext.Aktuels.Update(aktuel);
                    Console.WriteLine($"Updated {aktuel.Name} Aktuel");
                }

                List<AktuelPage> aktuelPages = new List<AktuelPage>();
                foreach (var page in latestItem.Links)
                {
                    var aktuelPage = new AktuelPage()
                    {
                        AktuelId = aktuel.Id,
                        OriginalImageUrl = page,
                        PageImageUrl = uploadService.UploadFile(page),
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    };
                    aktuelPages.Add(aktuelPage);
                    dbContext.AktuelPages.Add(aktuelPage);
                    queueService.AddQueue(QueueType.ContentUpdate, JsonConvert.SerializeObject(mapper.Map<AktuelPage, AktuelPageDto>(aktuelPage)));
                }
                Console.WriteLine($"Added {aktuelPages.Count} Aktuel Pages");
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"When process is running it has an error. {ex.Message}");
            }
        }

        void CreateOrUpdateCompany(Company companyEntity)
        {
            try
            {
                var company = dbContext.Companies.AsNoTracking().FirstOrDefault(x => x.Name == companyEntity.Name | x.CategoryId == companyEntity.CategoryId);
                if (company != null)
                {
                    companyEntity.Id = company.Id;
                    dbContext.Companies.Update(companyEntity);
                }
                else
                {
                    dbContext.Companies.Add(companyEntity);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CreateOrUpdate Company has Error: {ex.Message}");
            }
        }

        void CreateOrUpdateAktuel(Aktuel aktuelEntity)
        {
            try
            {
                var aktuel = dbContext.Aktuels.AsNoTracking().FirstOrDefault(x => x.NewsId == aktuelEntity.NewsId &
                                                                                x.CompanyId == aktuelEntity.CompanyId);
                if (aktuel != null)
                {
                    aktuelEntity.Id = aktuel.Id;
                    dbContext.Aktuels.Update(aktuelEntity);
                }
                else
                {
                    dbContext.Aktuels.Add(aktuelEntity);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CreateOrUpdate Aktuel has Error: {ex.Message}");
            }
        }

    }
}

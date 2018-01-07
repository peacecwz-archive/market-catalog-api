using System;
using System.Collections.Generic;
using AktuelListesi.Crawler.Models;

namespace AktuelListesi.Crawler.Interfaces
{
    public interface ICrawlerService
    {
        List<CrawlerItem> GetCompanies();
        List<CrawlerItem> GetLatest();
        List<CrawlerItem> GetAktuelPages(string categoryId);
    }
}

using AktuelListesi.AppService;
using AktuelListesi.AppService.Interfaces;
using AktuelListesi.Crawler;
using AktuelListesi.Crawler.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AktuelListesi
{
    public static class DIExtensions
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddTransient<ICrawlerService, CrawlerService>();
            services.AddTransient<IOneSignalService, OneSignalService>();
            services.AddTransient<IUploadService, UploadService>();
            services.AddTransient<IQueueService, QueueService>();
            services.AddTransient<ICognitiveService, CognitiveService>();

        }
    }
}

using AktuelListesi.Models.AppServices;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace AktuelListesi.Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();
            ConfigureServices(services);

            var configuration = new JobHostConfiguration();
            configuration.JobActivator = new CustomJobActivator(services.BuildServiceProvider());
            Console.WriteLine("Job Started");

            var host = new JobHost(configuration);
            host.RunAndBlock();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            bool IsDevelopment = true;
#if DEBUG
            IsDevelopment = true;
#else
            IsDevelopment = false;
#endif
            // Optional: Setup your configuration:
            var Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile((IsDevelopment) ? "appsettings.Development.json" : "appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            services.Configure<IConfigurationRoot>(Configuration);
            services.AddScoped<Functions, Functions>();

            #region Configuration

            services.Configure<CognitiveServiceOptions>(options =>
            {
                options.ServiceKey = Configuration["CognitiveService:ServiceKey"];
                options.ServiceUrl = Configuration["CognitiveService:ServiceUrl"];
                options.Language = Configuration["CognitiveService:Language"];
            });

            services.Configure<AzureStorageOptions>(options =>
            {
                options.ConnectionString = Configuration["AzureStorage:ConnectionString"];
                options.QueueName = Configuration["AzureStorage:QueueName"];
                options.ContainerName = Configuration["AzureStorage:ContainerName"];

            });

            #endregion

            services.AddRepositories(Configuration.GetConnectionString("AktuelDbConnection"));
            services.AddDataServices();
            services.AddAppServices();

            // One more thing - tell azure where your azure connection strings are
            Environment.SetEnvironmentVariable("AzureWebJobsDashboard", Configuration.GetValue<string>("AzureStorage:ConnectionString"));
            Environment.SetEnvironmentVariable("AzureWebJobsStorage", Configuration.GetValue<string>("AzureStorage:ConnectionString"));
        }

    }
}

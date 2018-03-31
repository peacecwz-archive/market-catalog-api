using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AktuelListesi.AppService;
using AktuelListesi.AppService.Interfaces;
using AktuelListesi.Crawler;
using AktuelListesi.Crawler.Interfaces;
using AktuelListesi.Repository;
using AktuelListesi.DataService;
using AktuelListesi.DataService.Implementations;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using AktuelListesi.Models.AppServices;

namespace AktuelListesi.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
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

            #region DbContext & Mapper

            services.AddDbContext<AktuelDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("AktuelDbConnection"), opt => opt.MigrationsAssembly("AktuelListesi.API"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }, contextLifetime: ServiceLifetime.Singleton, optionsLifetime: ServiceLifetime.Singleton);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });

            var mapper = config.CreateMapper();
            services.AddSingleton<IMapper>(mapper);

            #endregion

            #region DI Repository Service

            services.AddSingleton(typeof(IRepository<,>), typeof(Repository<,>));

            #endregion

            #region DI Data Service

            services.AddSingleton<IAktuelPageService, AktuelPageService>();
            services.AddSingleton<IAktuelService, AktuelService>();
            services.AddSingleton<ICompanyService, CompanyService>();

            #endregion

            #region DI App Services

            services.AddTransient<ICrawlerService, CrawlerService>();
            services.AddTransient<IOneSignalService, OneSignalService>();
            services.AddTransient<IUploadService, UploadService>();
            services.AddTransient<IQueueService, QueueService>();
            services.AddTransient<ICognitiveService, CognitiveService>();

            #endregion

            #region Swagger

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Aktuel Listesi API", Version = "v1" });
            });

            #endregion

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Aktuel Listesi V1");
            });

            app.UseMiddleware<Middlewares.ExceptionMiddleware>();

            app.UseMvc();
        }
    }
}

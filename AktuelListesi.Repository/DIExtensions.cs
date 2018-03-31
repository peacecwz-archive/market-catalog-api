using AktuelListesi.Repository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AktuelListesi
{
    public static class DIExtensions
    {
        public static void AddRepositories(this IServiceCollection services, string connectionString)
        {

            services.AddDbContext<AktuelDbContext>(options =>
            {
                options.UseNpgsql(connectionString, opt => opt.MigrationsAssembly("AktuelListesi.API"));
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }, contextLifetime: ServiceLifetime.Singleton, optionsLifetime: ServiceLifetime.Singleton);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });

            var mapper = config.CreateMapper();
            services.AddSingleton<IMapper>(mapper);

            services.AddSingleton(typeof(IRepository<,>), typeof(Repository<,>));

        }
    }
}

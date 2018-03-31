using AktuelListesi.DataService;
using AktuelListesi.DataService.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AktuelListesi
{
    public static class DIExtensions
    {
        public static void AddDataServices(this IServiceCollection services)
        {
            services.AddSingleton<IAktuelPageService, AktuelPageService>();
            services.AddSingleton<IAktuelService, AktuelService>();
            services.AddSingleton<ICompanyService, CompanyService>();
        }
    }
}

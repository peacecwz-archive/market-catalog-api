using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AktuelListesi.Updater
{
    public class CustomJobActivator : IJobActivator
    {
        private readonly IServiceProvider _service;
        public CustomJobActivator(IServiceProvider service)
        {
            _service = service;
        }

        public T CreateInstance<T>()
        {
            return _service.GetRequiredService<T>();
        }
    }

}

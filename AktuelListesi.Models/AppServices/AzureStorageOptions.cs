using System;
using System.Collections.Generic;
using System.Text;

namespace AktuelListesi.Models.AppServices
{
    public class AzureStorageOptions
    {
        public string ConnectionString { get; set; }
        public string QueueName { get; set; }
        public string ContainerName { get; set; }
    }
}

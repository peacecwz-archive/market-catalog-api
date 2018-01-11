using AktuelListesi.AppService.Interfaces;
using AktuelListesi.Models.AppServices;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace AktuelListesi.AppService
{
    public class QueueService : IQueueService
    {
        public AzureStorageOptions StorageOptions { get; set; }
        public QueueService(IOptions<AzureStorageOptions> storageOptions)
        {
            this.StorageOptions = storageOptions?.Value;
        }

        public QueueService(AzureStorageOptions storageOptions)
        {
            this.StorageOptions = storageOptions;
        }

        public QueueService() { }

        public bool AddQueue(string message)
        {
            try
            {
                CloudStorageAccount storage = CloudStorageAccount.Parse(StorageOptions.ConnectionString);
                var queueClient = storage.CreateCloudQueueClient();
                var queueRef = queueClient.GetQueueReference(StorageOptions.QueueName);

                queueRef.CreateIfNotExistsAsync().Wait();

                queueRef.AddMessageAsync(new Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage(message)).Wait();

                return true;
            }
            catch { return false; }
        }
    }
}

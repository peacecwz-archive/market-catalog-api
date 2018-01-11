using AktuelListesi.AppService.Interfaces;
using AktuelListesi.Models.AppServices;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace AktuelListesi.AppService
{
    public class QueueService : IQueueService, IDisposable
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

        ~QueueService()
        {
            Dispose();
        }

        public bool AddQueue(string message)
        {
            try
            {
                CloudStorageAccount storage = CloudStorageAccount.Parse(StorageOptions.ConnectionString);
                var queueClient = storage.CreateCloudQueueClient();
                var queueRef = queueClient.GetQueueReference(StorageOptions.QueueName);

                queueRef.CreateIfNotExistsAsync().Wait();

                queueRef.AddMessageAsync(new Microsoft.WindowsAzure.Storage.Queue.CloudQueueMessage(message)).Wait();

                GC.SuppressFinalize(queueRef);
                GC.SuppressFinalize(queueClient);
                GC.SuppressFinalize(storage);
                return true;
            }
            catch { return false; }
        }

        public void Dispose()
        {
            GC.WaitForPendingFinalizers();
        }

        public T GetNextQueueMessage<T>()
        {
            try
            {
                CloudStorageAccount storage = CloudStorageAccount.Parse(StorageOptions.ConnectionString);
                var queueClient = storage.CreateCloudQueueClient();
                var queueRef = queueClient.GetQueueReference(StorageOptions.QueueName);

                queueRef.CreateIfNotExistsAsync().Wait();

                var messageTask = queueRef.GetMessageAsync();
                messageTask.Wait();
                var message = messageTask.Result;

                var obj = JsonConvert.DeserializeObject<T>(message.AsString);
                queueRef.DeleteMessageAsync(message).Wait();

                GC.SuppressFinalize(messageTask);
                GC.SuppressFinalize(queueRef);
                GC.SuppressFinalize(queueClient);
                GC.SuppressFinalize(storage);
                return obj;
            }
            catch { return default(T); }
        }
    }
}

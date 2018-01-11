using AktuelListesi.Models.AppServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace AktuelListesi.AppService.Interfaces
{
    public interface IQueueService
    {
        AzureStorageOptions StorageOptions { get; set; }
        bool AddQueue(string message);
        T GetNextQueueMessage<T>();
    }
}

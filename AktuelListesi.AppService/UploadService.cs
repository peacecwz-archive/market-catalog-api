using AktuelListesi.AppService.Interfaces;
using AktuelListesi.Models.AppServices;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace AktuelListesi.AppService
{
    public class UploadService : IUploadService
    {
        public AzureStorageOptions StorageOptions { get; set; }
        public UploadService(IOptions<AzureStorageOptions> storageOptions)
        {
            this.StorageOptions = storageOptions?.Value;
        }

        public UploadService(AzureStorageOptions storageOptions)
        {
            this.StorageOptions = storageOptions;
        }

        public UploadService() { }
    }
}

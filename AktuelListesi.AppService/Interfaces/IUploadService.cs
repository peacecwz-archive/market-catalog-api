using AktuelListesi.Models.AppServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace AktuelListesi.AppService.Interfaces
{
    public interface IUploadService
    {
        AzureStorageOptions StorageOptions { get; set; }
    }
}

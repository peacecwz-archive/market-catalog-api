using AktuelListesi.Models.AppServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AktuelListesi.AppService.Interfaces
{
    public interface IUploadService
    {
        AzureStorageOptions StorageOptions { get; set; }
        string UploadFile(string fileUrl);
        string UploadFile(Stream stream);
        string UploadFile(byte[] data);
    }
}

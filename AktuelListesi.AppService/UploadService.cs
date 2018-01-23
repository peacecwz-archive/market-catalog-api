using AktuelListesi.AppService.Interfaces;
using AktuelListesi.Models.AppServices;
using FreeImageAPI;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Diagnostics;

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

        public string UploadFile(string fileUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                var requestTask = client.GetAsync(fileUrl);
                requestTask.Wait();
                var responseTask = requestTask.Result.Content.ReadAsStreamAsync();
                responseTask.Wait();
                if (requestTask.Result.IsSuccessStatusCode)
                    return UploadFile(responseTask.Result, "");
                return fileUrl;
            }
        }

        public string UploadFile(Stream stream)
        {
            return UploadFile(stream, "");
        }

        public string UploadFile(byte[] data)
        {
            using (MemoryStream st = new MemoryStream(data))
                return UploadFile(st, "");
        }

        private string UploadFile(Stream stream, string url = "")
        {
            try
            {
                using (var image = FreeImageBitmap.FromStream(stream))
                {
                    using (MemoryStream newStream = new MemoryStream())
                    {
                        image.Save(newStream, FREE_IMAGE_FORMAT.FIF_PNG, FREE_IMAGE_SAVE_FLAGS.PNG_Z_DEFAULT_COMPRESSION | FREE_IMAGE_SAVE_FLAGS.PNG_Z_BEST_SPEED);

                        CloudStorageAccount storage = CloudStorageAccount.Parse(StorageOptions.ConnectionString);
                        var blobClient = storage.CreateCloudBlobClient();
                        var blobContainer = blobClient.GetContainerReference(StorageOptions.ContainerName);
                        blobContainer.CreateIfNotExistsAsync().Wait();
                        var blobRef = blobContainer.GetBlockBlobReference(GenerateFileNmae(url));
                        blobRef.Properties.ContentType = "image/png";
                        blobRef.UploadFromByteArrayAsync(newStream.GetBuffer(),0,newStream.GetBuffer().Length).Wait();


                        return blobRef.Uri.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                return "";
            }
        }

        private string GenerateFileNmae(string url = "")
        {
            try
            {
                if (string.IsNullOrEmpty(url))
                    return Guid.NewGuid() + ".png";

                var uri = new Uri(url);
                return Path.GetFileName(uri.LocalPath);
            }
            catch (Exception ex)
            {
                Debug.Write(ex.Message);
                return Guid.NewGuid() + ".jpg";
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace SyncReadBlobTool
{
    public static class BlobStorageProvider
    {
        //获取blob数据
        public static async Task<string> GetBlobConent(string blobName, CloudBlobContainer blobContainer)
        {
            CloudAppendBlob blob = blobContainer.GetAppendBlobReference($"{(object)blobName}.json");
            bool flag = await blob.ExistsAsync();
            if (flag)
            {
                return await blob.DownloadTextAsync();
            }
            else
                return "";
        }

        public static CloudStorageAccount CloudStorageAccount => CloudStorageAccount.Parse(ConfigurationManager.AppSettings["BlobConnectionString"]);

        public static async Task<CloudBlobContainer> InitAsync()
        {
            CloudBlobClient blobClient = CloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = blobClient.GetContainerReference("oldsync-grainstate");
            await blobContainer.CreateIfNotExistsAsync();
            return blobContainer;
        }

        public static async Task WriteStateToBlob(CloudBlobContainer blobContainer, string blobName, string content)
        {
            CloudAppendBlob appendBlob = blobContainer.GetAppendBlobReference($"{(object)blobName}.json");

            bool flag = await appendBlob.ExistsAsync();
            if (!flag)
                await appendBlob.CreateOrReplaceAsync();
            appendBlob.Properties.ContentType = "application/json";
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                stream.Position = 0;
                await appendBlob.UploadFromStreamAsync(stream);
            }
        }
    }
}

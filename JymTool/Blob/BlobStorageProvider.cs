using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using J.Base.Lib;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Orleans;
using Orleans.Providers;
using Orleans.Runtime;
using Orleans.Storage;

namespace JymTool
{
    public static class BlobStorageProvider
    {
        //获取blob数据
        public static async Task<string> GetBlobConent(string blobName, CloudBlobContainer blobContainer)
        {
            CloudAppendBlob blob = blobContainer.GetAppendBlobReference($"{(object)blobName}.json");
            bool flag = await blob.ExistsAsync();
            bool exist = flag;
            string str1;
            if (exist)
            {
                string str2 = await blob.DownloadTextAsync();
                object obj = (object)str2;
                str1 = $"[{obj}]";
            }
            else
                str1 = "";
            return str1;
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


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types
//using Microsoft.IdentityModel.Protocols;
using System.Configuration;
using System.Globalization;

namespace Bot_Application1.Controllers
{
    public class MediaController
    {
        CloudStorageAccount storageAccount;
         CloudBlobClient blobClient;
         CloudBlobContainer container;

        public MediaController()
        {
          

            
             var conf = ConfigurationManager.AppSettings.Get("mrhmedia_AzureStorageConnectionString");

            storageAccount = CloudStorageAccount.Parse(conf);
             blobClient = storageAccount.CreateCloudBlobClient();
             container = blobClient.GetContainerReference("pics");

        }

        public string getFileUrl(string key)
        {
            try
            {
                CloudBlockBlob blockBlob;
                blockBlob = container.GetBlockBlobReference(key);
                var sas = blockBlob.GetSharedAccessSignature(new SharedAccessBlobPolicy()
                {
                    Permissions = SharedAccessBlobPermissions.Read,
                    SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15),
                });

                string sasUrl = string.Format(CultureInfo.InvariantCulture, "{0}{1}", blockBlob.Uri, sas);
                return sasUrl;

            }
            catch (Exception ex)
            {
                return "";
            }

        }



    }
}
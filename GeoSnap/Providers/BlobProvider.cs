using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Drawing;
using System.IO;

namespace GeoSnap.Providers
{
    public class BlobProvider
    {
        CloudStorageAccount storageAccount;
        CloudBlobClient blobClient;
        CloudBlobContainer container;

        public void save(MemoryStream ms,int id) {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("BlobConnectionString"));
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("img");
            //BlobContainerPermissions permissions = container.GetPermissions();
            //permissions.PublicAccess = BlobContainerPublicAccessType.Container;
            //container.SetPermissions(permissions);
            CloudBlockBlob b = container.GetBlockBlobReference(id.ToString() + ".jpg");
            using (MemoryStream memStream = ms) {
                b.UploadFromStream(ms);
            }
        }
    }
}
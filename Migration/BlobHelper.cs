using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;
using System.IO;

namespace Migration
{
    public class CYCBlobHelper
    {
        public static void Rename(string storename, string oldFileName, string newFileName)
        {
            var blob = GetPublicContainer(storename).GetBlobReference(oldFileName);

            try
            {
                //trying to see if the blob exists. If it doesnt exist it will throw an exception.               
                blob.FetchAttributes();

                MemoryStream imageStream = new MemoryStream();
                blob.DownloadToStream(imageStream);
                imageStream.Position = 0;

                var originalFileName = blob.Metadata["FileName"];
                var contentType = blob.Properties.ContentType;

                var newBlob = GetPublicContainer(storename).GetBlobReference(newFileName);
                newBlob.UploadFromStream(imageStream);
                if (!string.IsNullOrEmpty(originalFileName)) newBlob.Metadata["FileName"] = originalFileName;
                newBlob.SetMetadata();
                newBlob.Properties.ContentType = contentType;
                newBlob.SetProperties();
            }
            catch
            { }
        }

        public static CloudBlob GetBlob(string storename, string url)
        {
            var filename = Path.GetFileName(url);
            var blob = GetPublicContainer(storename).GetBlobReference(filename);

            try
            {
                //trying to see if the blob exists. If it doesnt exist it will throw an exception.               
                blob.FetchAttributes();
                return blob;
            }
            catch { }
            return null;
        }

        private static string PUBLIC_DATA_STORE = "PublicDataStore";
        private static CloudBlobContainer GetPublicContainer(string name)
        {
            //CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
            //        configSetter(ConfigurationManager.AppSettings[configName]));

            var cloudStorageAccount = CloudStorageAccount.FromConfigurationSetting(PUBLIC_DATA_STORE);

            var blobClient = cloudStorageAccount.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference(name);

            container.CreateIfNotExist();

            var permissions = new BlobContainerPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Container;
            container.SetPermissions(permissions);

            return container;
        }

    }
}
